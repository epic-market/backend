using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class OrdersDto
    {
        public int OutletId { get; set; }
        public string PaymentMode { get; set; }
        public int StatusId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public  List<OrderDetailsDto> orderDetailsDtos { get; set; } // this List<OrderDetailsDto> convert to json to string
    }

    public class CreateCustomerOrderDto
    {
        public int OutletId { get; set; }
        public string PaymentMode { get; set; }
        public List<OrderDetailsDto> OrderDetailsDtos { get; set; } // Required for order details
    }


    public class OrdersDetailsResult
    {
        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string PaymentMode { get; set; }

        public string OrderMode { get; set; }

        public string Status { get; set; }

        public double TotalPrice { get; set; }

        public int TotalItems { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } 
    }




    public class OrderParams
    {
        public int? BranchId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public bool ascending { get; set; } = true;
        public string searchTerm { get; set; } = string.Empty;

        public int? statusID { get; set; }

    }

    public class OrderResult
    {
        public int ID { get; set; }

        public string CustomerName { get; set; }

        public string Status { get; set; }

        public double TotalPrice { get; set; }

        public int TotalItems { get; set; }

        public string OrderType { get; set; }

        public int Count { get; set; }
    }

    public class OrderMobileResult
    {
        public int ID { get; set; }

        public string Status { get; set; }
        public string Payment_Mode { get; set; }
        public DateTime? Ordered_At { get; set; } 
        public OrderBranchMobileResult Branch {get;set;}
        public OrderCustomerMobileResult Customer { get;set;}
        public OrderItemMobileResult Items_Peak { get; set; }
        public double? Items_count { get; set; }
        public double? Total_price { get; set; }

    }
    public class OrderMobileDeatilsResult
    {
        public int ID { get; set; }

        public string Status { get; set; }
        public string Payment_Mode { get; set; }
        public DateTime? Ordered_At { get; set; } 
        public OrderBranchMobileResult Branch {get;set;}
        public OrderCustomerMobileResult Customer { get;set;}
        public List<OrderItemMobileResult> Items { get; set; }
        public double? Items_count { get; set; }
        public double? Total_price { get; set; }

    }
    public class OrderBranchMobileResult
    {
        public int? ID { get; set; }
        public string Name { get; set; }
    } 
    public class OrderCustomerMobileResult
    {
        public int? ID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    } 
    public class OrderItemMobileResult
    {
        public int? ID { get; set; }
        public int? Quantity { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public double? Total_price { get; set; }
    }

    public class OrderHistoryRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StatusId { get; set; }
        public string SortBy { get; set; }

        public string SortOrder { get; set; }   
    }

    public class CustomerOrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public double TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public string OutletName { get; set; }
        public int OutletId { get; set; }
        public string PaymentMethod { get; set; }
    }

}
