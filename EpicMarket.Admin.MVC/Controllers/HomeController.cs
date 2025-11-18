using EpicMarket.Admin.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EpicMarket.Data.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT},{ROLES.ADMIN},{ROLES.SUPPORT}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Get user information
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            int userIdInt = 0;
            if (!string.IsNullOrEmpty(userId))
            {
                userIdInt = int.Parse(userId);
            }

            // Determine greeting based on time of day
            string greetingMessage = GetGreetingByTime();

            // Create dashboard view model
            var dashboardViewModel = new DashboardViewModel
            {
                // Get business metrics
                BusinessCount = _context.Businesses.Count(),
                OutletCount = _context.Outlets.Count(),
                OrderCount = _context.Orders.Count(),
                TotalRevenue = _context.Orders.Sum(o => o.TotalPrice),
                
                // User information
                UserFirstName = user?.FirstName ?? "User",
                GreetingMessage = greetingMessage,
                
                // Get user's recent tasks
                RecentTasks = _context.Tasks
                    .Where(t => t.PrimaryAssignedToPersonID == userIdInt)
                    .OrderByDescending(t => t.CreateDate)
                    .Take(5)
                    .Include(t => t.TaskStatusType)
                    .Include(t => t.TaskTypes)
                    .ToList()
            };

            return View(dashboardViewModel);
        }

        private string GetGreetingByTime()
        {
            int hour = DateTime.Now.Hour;

            if (hour < 12)
            {
                return "Good Morning";
            }
            else if (hour < 18)
            {
                return "Good Afternoon";
            }
            else
            {
                return "Good Evening";
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
