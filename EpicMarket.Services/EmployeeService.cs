using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NVelocity;
using NVelocity.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IApplicationConfigurationService applicationConfiguration;
        private readonly UserManager<AppUser> userManager;
        private readonly IAddressService addressService;
        private readonly ICommunicationService communicationService;

        public EmployeeService(ApplicationDbContext context, IMapper mapper , IApplicationConfigurationService applicationConfiguration,ICommunicationService communicationService, UserManager<AppUser> userManager,IAddressService addressService)
        {
            _context = context;
            this.mapper = mapper;
            this.applicationConfiguration = applicationConfiguration;
            this.userManager = userManager;
            this.addressService = addressService;
            this.communicationService = communicationService;
        }
        public async Task<AddEmployeeResult> Register(AddEmployeeParam addEmployeeParam, int businessid)
        {
            var employee = new AddEmployeeResult();

            var UniqueGuid = Guid.NewGuid();

            var user = new AppUser() { FirstName = addEmployeeParam.FirstName , UniqueGuid = UniqueGuid.ToString() };

            if (await UserExists(addEmployeeParam.EmailID)) return BadRequest("Username is taken");

            user.UserName = addEmployeeParam.EmailID.ToLower();

            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault().ToString());

            var roleResult = await userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors.FirstOrDefault().ToString());


            string queryToken = user.Id + "." + UniqueGuid;

            var EmailTemplete = applicationConfiguration.GetApplicationConfigurationValue("BusinessOwnerInvitation");

            var Business = _context.Businesses.Find(businessid);

            var User = _context.Users.Find(addEmployeeParam.UserID);

            var _ =  _context.BusinessEmployeeMaps.Add(new BusinessEmployeeMap() {  BussinessID  = businessid, EmployeeID = user.Id});
            
            await _context.SaveChangesAsync();
            
            Hashtable ListValues = new Hashtable();

            ListValues.Add("fromAddress", addEmployeeParam.FirstName);
            ListValues.Add("BussinessName", Business.Name);
            ListValues.Add("BusinessOwnerName", User.FirstName);
            ListValues.Add("RedirectUrl", "https://owner.epicmarket.in/registerEmployee/"+ queryToken);

            VelocityEngine v = new VelocityEngine();
            v.Init();

            VelocityContext context = new VelocityContext(ListValues);
            StringWriter writer = new StringWriter();
            v.Evaluate(context, writer, string.Empty, EmailTemplete);
            var message = writer.ToString();

            communicationService.SendEmailAsync(addEmployeeParam.EmailID, "Welcome to Epic Market", message);

			employee.UserID = user.Id;
            employee.FirstName = user.FirstName;


			return employee;


        }

        private AddEmployeeResult BadRequest(string v)
        {
            throw new Exception(v);
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        public CheckLinkResult CheckEmployeeLink(string queryParam)
        {
            string[] tokenParts = queryParam.Split('.');

            string userId = tokenParts[0];
            string uniqueGuid = tokenParts[1];
            int UserID;
            int.TryParse(userId, out UserID);
            var User = _context.Users.Find(UserID);

            if (User.UniqueGuid == uniqueGuid)
            {
                var Business = _context.BusinessEmployeeMaps.Where(a => a.EmployeeID == User.Id).Include(a => a.Bussiness).FirstOrDefault();
                var result = new CheckLinkResult()
                {

                    UserID = userId,
                    FirstName = User.FirstName,
                    BusinessName = Business.Bussiness.Name,
                    BusinessEmail = Business.Bussiness.ContactEmail,
                };

                return result;
            }else
            {
                throw new Exception("Link is Not correct");
            }
        }

        public async Task<int> CreateEmployeeAccount(EmployeeDto employee)
        {
            var address = new AddressDto()
            {
                Address1 =  employee.Address,
                City = employee.City,
                State =employee.State,
                Pincode = employee.Pincode,
            };

            var addressId = await addressService.AddAddress(address);

            var User = _context.Users.Where(u => employee.ID == u.Id).FirstOrDefault();
            User.FirstName = employee.FirstName;
            User.LastName = employee.LastName;
            User.Email = employee.Email;
            User.PhoneNumber = employee.ContactNumber;
            User.UniqueGuid = null;
            await userManager.AddPasswordAsync(User, employee.Password);
            _context.Users.Update(User);
            _context.SaveChanges();

            var userAddress = new UserAddress() {
                AddressId = addressId,
                UserId = User.Id,
            };

			await userManager.AddToRoleAsync(User, "businessEmployee");

			_context.UserAddresses.Add(userAddress);
            _context.SaveChanges();

            return User.Id;

        }

        public async Task<List<EmployeeMapOptionResult>> GetAllEmployeesForMap(int businessid, int Outletid)
        {
            var _ = await (from singleEmployee in _context.BusinessEmployeeMaps.Include(e => e.Employee)
                          join OutletPerson in (_context.OutletPeople.Where(a => a.OutletId == Outletid))
                          on singleEmployee.EmployeeID equals OutletPerson.PersonId into joinedEmployees
                          from matchedEmployee in joinedEmployees.DefaultIfEmpty()
                          where singleEmployee.BussinessID == businessid
                          select new EmployeeMapOptionResult
                          {
                              Id = singleEmployee.EmployeeID,
                              Name = singleEmployee.Employee.FirstName,
                              Email = singleEmployee.Employee.Email,
                              Selected = matchedEmployee == null ? 0 : 1,
                          }).ToListAsync();

            return _;
        }

        public async Task<List<EmployeeResult>> GetAllEmployees(EmployeeParams employeeParams, int businessid)
        {

            //1 . filter with BusinessID
            var Employess = _context.BusinessEmployeeMaps
                                .Include(c=> c.Employee).Where(c => c.BussinessID == businessid);


            //2 . Appling Searching
            var searchedEmployees = Employess.Where(
                row => row.Employee.FirstName.Contains(employeeParams.searchTerm) ||
                (row.Employee.Id).ToString() == employeeParams.searchTerm);


            var sortedEmployess = searchedEmployees;
            // 3 .Appying Sorting
            switch (employeeParams.sortColumn)
            {
                case "EmployeeID":
                    sortedEmployess = employeeParams.ascending ? searchedEmployees.OrderBy(c => c.Employee.Id) : searchedEmployees.OrderByDescending(c => c.Employee.Id);
                    break;
                case "Name":
                    sortedEmployess = employeeParams.ascending ? searchedEmployees.OrderBy(c => c.Employee.FirstName) : searchedEmployees.OrderByDescending(c => c.Employee.FirstName);
                    break;
                default:
                    break;
            }

            //getting the total count
            int totalCount = sortedEmployess.Count();


            // 4. Apply pagination (skip and take)
            var pagedOutlets = sortedEmployess
                .Skip((employeeParams.PageIndex - 1) * employeeParams.pageSize) // Skip items for previous pages
                .Take(employeeParams.pageSize); // Take items for the current page

            // 5. Select data and add SRNO
            var results = await pagedOutlets.Select(c => new EmployeeResult()
            {
                EmployeeID = c.Employee.Id,
                FirstName = c.Employee.FirstName,
                ContactNumber = c.Employee.PhoneNumber,
                EmailId = c.Employee.Email,
                Count = totalCount
            }).ToListAsync();

            return results;
        }

        public async Task<SingleEmployeeResult> GetEmployeeDetails(int employeeId)
        {
           return await _context.UserAddresses.Where(c=>c.UserId == employeeId).Include(c => c.User).Include(c=>c.Address).Select(
               c=> new SingleEmployeeResult()
               { 
                 EmployeeID = c.User.Id,
                 EmailId= c.User.Email,
                 FirstName = c.User.FirstName,
                 LastName = c.User.LastName,
                 ContactNumber= c.User.PhoneNumber,
                 Address = c.Address.Address1,
                 City = c.Address.City,
                 State = c.Address.State,
                 PinCode = c.Address.Pincode
               }).FirstOrDefaultAsync();
        }
    }
}
