using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class BusinessModel_Params :Business
    {
        public IFormFile[] Images { get; set; } // Update this property
    }
}
