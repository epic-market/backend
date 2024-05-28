using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class OrderDetailsModel
    {
        public Order Order { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public OrderDetail OrderDetail { get; set; }
    }
}
