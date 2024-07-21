using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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
        public List<AccessControlList_Result> GetAccessControlList(Profile_SearchParams searchParams)
        {
           var roleID = context.Roles.FirstOrDefault(r => r.Name == searchParams.UserRole).Id;
           var AccessControlList = context.AccessControlLists.
                Include(a=> a.Securable).
                Include(a => a.AccessType).
                Where(a => a.AccessType.Name ==  Constants.ACCESSTYPE_READWRITE && a.RoleID == roleID).
                Select(a => new AccessControlList_Result()
                 {
                     ID = a.ID,
                     AccessType = a.AccessType.Name,
                     SecurableName = a.Securable.Name,
                 }).ToList();

            return AccessControlList;
        }
    }
}
