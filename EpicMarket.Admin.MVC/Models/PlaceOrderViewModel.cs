public class PlaceOrderViewModel
{
    public int OutletId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public string PaymentMode { get; set; }
    public int TotalItems { get; set; }
    public double TotalPrice { get; set; }
    public List<OrderDetailViewModel> OrderDetails { get; set; }
}

public class OrderDetailViewModel
{
    public int VariantId { get; set; }
    public int Quantity { get; set; }
    public double Rate { get; set; }
    public double TotalPrice { get; set; }
} 