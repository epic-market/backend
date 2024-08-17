using Amazon.Runtime.Internal.Util;
using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
		private readonly ICommunicationQueueService communicationQueueService;
		private readonly IUnitOfWork unitOfWork;
		private readonly ICommunicationService communicationService;
        private readonly IEventLogService eventLogService;

        public EmployeeService(ApplicationDbContext context,
            IMapper mapper,
            IApplicationConfigurationService applicationConfiguration,
            ICommunicationService communicationService,
            UserManager<AppUser> userManager,
            IAddressService addressService,
            ICommunicationQueueService communicationQueueService,
            IUnitOfWork unitOfWork,
            IEventLogService eventLogService)
        {
            _context = context;
            this.mapper = mapper;
            this.applicationConfiguration = applicationConfiguration;
            this.userManager = userManager;
            this.addressService = addressService;
			this.communicationQueueService = communicationQueueService;
			this.unitOfWork = unitOfWork;
			this.communicationService = communicationService;
            this.eventLogService = eventLogService;
        }
        public async Task<AddEmployeeResult> Register(AddEmployeeParam addEmployeeParam, int businessid , int userID)
        {
            var employee = new AddEmployeeResult();

            var UniqueGuid = Guid.NewGuid();

            var user = new AppUser() { FirstName = addEmployeeParam.FirstName , UniqueGuid = UniqueGuid.ToString() };

            if (await UserExists(addEmployeeParam.EmailID)) return BadRequest("Username is taken");

            user.UserName = addEmployeeParam.EmailID.ToLower();
			user.Email = addEmployeeParam.EmailID.ToLower();

			var result = await userManager.CreateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault().ToString());

            var roleResult = await userManager.AddToRoleAsync(user, ROLES.MEMBER);

            if (!roleResult.Succeeded) return BadRequest(result.Errors.FirstOrDefault().ToString());


            string queryToken = user.Id + "." + UniqueGuid;

            var EmailTemplete = applicationConfiguration.GetApplicationConfigurationValue("BusinessOwnerInvitation");

            var Business = _context.Businesses.Find(businessid);

            var User = _context.Users.Find(userID);

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

			await this.communicationQueueService.InsertCommunicationQueue(
				  new Entities.CommunicationQueueDTO()
				  {
					  MessageData = message,
					  Subject = "Welcome to Epic Market",
					  NotificationRecipient = addEmployeeParam.EmailID,
					  ContactMethod = ContactMethodConstants.EMAIL,
					  CreateBy = addEmployeeParam.EmailID
				  });

			//communicationService.SendEmailAsync(addEmployeeParam.EmailID, "Welcome to Epic Market", message);

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
                var Business = _context.BusinessEmployeeMaps.Where(a => a.EmployeeID == User.Id && a.IsActive == true).Include(a => a.Bussiness).FirstOrDefault();
                var result = new CheckLinkResult()
                {

                    UserID = userId,
                    FirstName = User.FirstName,
                    BusinessName = Business.Bussiness.Name,
                    Email = User.Email,
                    BusinessEmail = Business.Bussiness.ContactEmail,
                };

                return result;
            }else
            {
                throw new Exception("Link is Not correct");
            }
        }

        public async Task<int> CreateEmployeeAccount(EmployeeDto employee , int id)
        {

            var User = _context.Users.Where(u => id == u.Id).FirstOrDefault();

            if (User.UniqueGuid == employee.Guid) {
                var address = new AddressDto()
                {
                    Address1 = employee.Address,
                    City = employee.City,
                    State = employee.State,
                    Pincode = employee.Pincode,
                };

                var addressId = await addressService.AddAddress(address);
                User.FirstName = employee.FirstName;
                User.LastName = employee.LastName;
                User.PhoneNumber = employee.ContactNumber;
                User.UniqueGuid = null;
                await userManager.AddPasswordAsync(User, employee.Password);
                _context.Users.Update(User);
				await unitOfWork.Complete();

				var userAddress = new UserAddress()
                {
                    AddressId = addressId,
                    UserId = User.Id,
                };

                await userManager.AddToRoleAsync(User, ROLES.BUSINESS_EMPLOYEE);

                _context.UserAddresses.Add(userAddress);
				await unitOfWork.Complete();
				var saved = _context.Users.FirstOrDefault(o => o.Id == User.Id);
                string savedJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = User.Id, Data = savedJson, Description = null, EventName = EventConstants.AddEmployees, EntityName = EntityConstants.Employees });

                return User.Id;

            }
            else
            {
                throw new Exception("Invalid Link");
            }
        }

        public async Task<List<EmployeeMapOptionResult>> GetAllEmployeesForMap(int businessid, int Outletid)
        {
            var _ = await (from singleEmployee in _context.BusinessEmployeeMaps.Include(e => e.Employee)
                          join OutletPerson in (_context.OutletPeople.Where(a => a.OutletId == Outletid))
                          on singleEmployee.EmployeeID equals OutletPerson.PersonId into joinedEmployees
                          from matchedEmployee in joinedEmployees.DefaultIfEmpty()
                          where singleEmployee.BussinessID == businessid && singleEmployee.IsActive == true
                          select new EmployeeMapOptionResult
                          {
                              Id = singleEmployee.EmployeeID,
                              Name = singleEmployee.Employee.FirstName,
                              Email = singleEmployee.Employee.Email,
                              Selected = matchedEmployee == null ? false : true,
                          }).ToListAsync();

            return _;
        }

        public async Task<GetDataResult<List<EmployeeResult>>> GetAllEmployees(EmployeeParams employeeParams, int businessid)
        {


            var getResult = new GetDataResult<List<EmployeeResult>>();
            //1 . filter with BusinessID
            var Employess = _context.BusinessEmployeeMaps
                                .Include(c=> c.Employee).Where(c => c.BussinessID == businessid && c.IsActive == true);


            //2 . Appling Searching
            var searchedEmployees = Employess.Where(
                row => row.Employee.FirstName.Contains(employeeParams.searchTerm.Trim()) ||
                (row.Employee.Id).ToString() == employeeParams.searchTerm.Trim());


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

            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

        public async Task<SingleEmployeeResult> GetEmployeeDetails(int employeeId)
        {

			var singleEmployeeResults = from businessEmployee in _context.BusinessEmployeeMaps
										join user in _context.Users on businessEmployee.EmployeeID equals user.Id
										join userAddress in _context.UserAddresses on businessEmployee.EmployeeID equals userAddress.UserId into userAddressGroup
										from userAddress in userAddressGroup.DefaultIfEmpty() // Left join on UserAddresses
										join address in _context.Addresses on userAddress.AddressId equals address.Id into addressGroup
										from address in addressGroup.DefaultIfEmpty() // Left join on Addresses
										where businessEmployee.EmployeeID == employeeId && businessEmployee.IsActive == true
										select new SingleEmployeeResult
										{
											EmployeeID = businessEmployee.EmployeeID,
											FirstName = user.FirstName,
											LastName = user.LastName,
											ContactNumber = user.PhoneNumber,
											EmailId = user.Email,
											Address = address != null ? address.Address1 : null,
											City = address != null ? address.City : null,
											State = address != null ? address.State : null,
											PinCode = address != null ? address.Pincode : null,
										};


			return await singleEmployeeResults.FirstOrDefaultAsync();


       }

		public async Task DeleteEmployee(int employeeId)
		{
			var businessEmployeeMapEmployee = await _context.BusinessEmployeeMaps.FirstOrDefaultAsync(c => c.EmployeeID == employeeId && c.IsActive == true);
			var AppUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == employeeId && c.IsActive == true);

			if (businessEmployeeMapEmployee != null)
			{
				businessEmployeeMapEmployee.IsActive = false;
				_context.BusinessEmployeeMaps.Update(businessEmployeeMapEmployee);
				_context.Users.Update(AppUser);
				await unitOfWork.Complete();
			}
			else
			{
				throw new Exception("Employee Not Found");
			}

		}
	}
}
