using System;
using System.Collections.Generic;

namespace EpicMarket.Admin.MVC.Models
{
    public class InventoryFilterViewModel
    {
        public int? BusinessID { get; set; }
        public int? OutletID { get; set; }
        public int? ProductID { get; set; }
        public int? ProductVariantID { get; set; }
        public string ProductName { get; set; }
        
        // Pagination properties
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        // Sorting properties
        public string SortColumn { get; set; } = "ID";
        public string SortDirection { get; set; } = "desc";
    }
    
    public class InventoryDto
    {
        public int ID { get; set; }
        public int OutletID { get; set; }
        public string OutletName { get; set; }
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public int ProductVariantID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public bool TrackInventory { get; set; }
        public bool IsInStock { get; set; }
        public int? QuantityAvailable { get; set; }
        public int? MinimumStockLevel { get; set; }
        public int? MaximumStockLevel { get; set; }
        public int? ReorderPoint { get; set; }
        public bool BackOrders { get; set; }
    }
} 