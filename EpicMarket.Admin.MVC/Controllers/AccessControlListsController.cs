using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class AccessControlListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessControlListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var roles = _context.Roles.ToList();
            var securables = _context.ApplicationSecurables.ToList();
            var accessTypes = _context.AccessTypes.ToList();

            // Assume "Denied" has ID 0
            var deniedAccessTypeId = 0;

            var viewModel = new SecurityMatrixViewModel
            {
                RoleNames = roles.Select(r => r.Name).ToList(),
                Securables = securables.Select(s => new SecurableViewModel
                {
                    Name = s.Name,
                    RoleAccess = roles.ToDictionary(
                        r => r.Name,
                        r => _context.AccessControlLists
                            .FirstOrDefault(rs => rs.RoleID == r.Id && rs.SecurableID == s.Id)?.AccessTypeID ?? deniedAccessTypeId
                    )
                }).ToList(),
                AccessTypes = accessTypes.Select(at => new AccessTypeViewModel
                {
                    ID = at.Id,
                    Name = at.Name
                }).ToList(),
                DefaultAccessTypeId = deniedAccessTypeId
            };

            return View(viewModel);


            //var authDbContext = _context.AccessControlLists.Include(a => a.AccessType).Include(a => a.Role).Include(a => a.Securable);
            //return View(await authDbContext.ToListAsync());
        }


        [HttpPost]
        public IActionResult UpdateAccessType(string securable, string role, int accessTypeId)
        {
            try
            {
                // Find the corresponding RoleSecurable entry and update it
                var roleSecurable = _context.AccessControlLists
                    .FirstOrDefault(rs => rs.Securable.Name == securable && rs.Role.Name == role);

                if (roleSecurable == null)
                {
                    // Create a new entry if it doesn't exist
                    roleSecurable = new AccessControlList
                    {
                        Role = _context.Roles.FirstOrDefault(r => r.Name == role),
                        Securable = _context.ApplicationSecurables.FirstOrDefault(s => s.Name == securable),
                        AccessTypeID = accessTypeId
                    };
                    _context.AccessControlLists.Add(roleSecurable);
                }
                else
                {
                    roleSecurable.AccessTypeID = accessTypeId;
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = "An error occurred while updating the access type." });
            }
        }

   

     
    }
}
