namespace EpicMarket.Admin.MVC.Models
{
    public class BusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Branch> Branches { get; set; }
    }

    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public BusinessModel Business { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class SingleOrder
    {
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public string PaymentMode { get; set; }
        public CustomerDetails CustomerDetails { get; set; }
    }

    public class CustomerDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
