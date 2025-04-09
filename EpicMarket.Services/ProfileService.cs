using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext context;

        public ProfileService(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Retrieves the access control list for a given user based on their roles.
        /// </summary>
        /// <param name="searchParams">Search parameters including the logged in user's name.</param>
        /// <returns>A list of access control list results.</returns>
        public List<AccessControlList_Result> GetAccessControlList(Profile_SearchParams searchParams)
        {
            if (searchParams == null || string.IsNullOrEmpty(searchParams.LoggedInUserName))
            {
                throw new ArgumentNullException(nameof(searchParams), "Search parameters cannot be null or empty.");
            }

            var roles = context.UserRoles
                .Include(c => c.User)
                .Where(c => c.User.UserName == searchParams.LoggedInUserName)
                .Select(c => c.RoleId)
                .ToList();

            if (!roles.Any())
            {
                throw new Exception($"No roles found for user {searchParams.LoggedInUserName}.");
            }

            var accessControlList = context.AccessControlLists
                .Include(a => a.Securable)
                .Include(a => a.AccessType)
                .Where(a => a.AccessType.Name == Constants.ACCESSTYPE_READWRITE && roles.Contains(a.RoleID))
                .Select(a => new
                {
                    securable = a.Securable.Name,
                    accessType = a.AccessType.Name
                })
                .Distinct()
                .Select(a => new AccessControlList_Result
                {
                    SecurableName = a.securable,
                    AccessType = a.accessType
                })
                .ToList();

            return accessControlList;
        }

        /// <summary>
        /// Retrieves customer details based on phone number or user name.
        /// </summary>
        /// <param name="PhoneOrUserName">Phone number or user name to search for.</param>
        /// <returns>A list of customer details.</returns>
        public async Task<List<CustomerDetails>> GetCustomerDetails(string PhoneOrUserName)
        {
            if (string.IsNullOrEmpty(PhoneOrUserName))
            {
                throw new ArgumentNullException(nameof(PhoneOrUserName), "Phone or user name cannot be null or empty.");
            }

            var query = context.Users
                .Where(customer => customer.UserName.ToLower().Contains(PhoneOrUserName.ToLower()) ||
                                 customer.PhoneNumber.ToString().ToLower().Contains(PhoneOrUserName.ToLower()))
                .Select(c => new CustomerDetails()
                {
                    FirstName = c.FirstName,
                    PhoneNumber = c.PhoneNumber,
                    Username = c.UserName,
                });

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Retrieves basic customer details asynchronously.
        /// </summary>
        /// <param name="UserName">User name to retrieve details for.</param>
        /// <returns>A customer basic details DTO.</returns>
        public async Task<CustomerBasicDetailsDto> GetCustomerBasicDetailsAsync(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                throw new ArgumentNullException(nameof(UserName), "User name cannot be null or empty.");
            }

            var AttachmentType = await context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.Profile);
            if (AttachmentType == null)
            {
                throw new Exception("Profile attachment type not found.");
            }

            var customerDetails = await context.Users
                .Where(c => c.UserName == UserName)
                .Select(c => new CustomerBasicDetailsDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    ProfilePhoto = (from attachment in context.Attachments
                                    join link in context.AttachmentLinks
                                    on attachment.ID equals link.AttachmentID
                                    join entity in context.Entity
                                    on link.EntityID equals entity.ID
                                    where entity.Name == EntityConstants.Profile
                                    && link.RecordID == c.Id
                                    && link.AttachmentTypeID == attachment.ID
                                    select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}").FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            return customerDetails;
        }
    }
}
