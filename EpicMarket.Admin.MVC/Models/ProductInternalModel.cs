namespace EpicMarket.Admin.MVC.Models
{
    public class ProductInternalModel
    {
        public int ID { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile[] Images { get; set; } // Update this property
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
