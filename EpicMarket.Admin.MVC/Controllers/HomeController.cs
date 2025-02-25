using EpicMarket.Admin.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Data.Models;
using System.Security.Claims;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT},{ROLES.ADMIN},{ROLES.SUPPORT}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.Firstname = this.context.Users.FirstOrDefault(u => u.UserName == userName).FirstName;
            int hour = DateTime.Now.Hour;

            if (hour < 12)
            {
                ViewBag.TypeofMessage = "Good Morning";
            }
            else if (hour < 18)
            {
                ViewBag.TypeofMessage = "Good Afternoon";
            }
            else
            {
                ViewBag.TypeofMessage = "Good Evening";
            }



            return View();

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
