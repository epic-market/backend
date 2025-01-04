using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{


	public class AddProductsDto
	{
        public long? Barcode { get; set; }
        [Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }
		public string Category { get; set; }
		
        [Required]
		public double Rate { get; set; }
		public bool IsRecommended { get; set; }= false;
		public int? MaximumOrderPurchase { get; set; }

        [Required]
        public double CostPrice { get; set; }

        public double? PackedHeight { get; set; }
        public double? PackedWidhth { get; set; }
        public double? PackedDepth { get; set; }
        public double? Weight { get; set; }
        public bool RequiresRefrigeration { get; set; } = false;
        public IFormFile[] Products { get; set; }
		public IFormFile Thumbnail { get; set; }
        public string Variants { get; set; }
	}


    

	public class ProductsDto
    {
        public int? Id { get; set; }
		public long? Barcode { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
        public string Category{ get; set; }
        public double Rate { get; set; }
        public List<string> Images { get; set; } 
		public bool IsRecommended { get; set; }
		public int? MaximumOrderPurchase { get; set; }
        public string? Status { get; set; }
		public string Thumbnail { get; set; }
        public double CostPrice { get; set; }
        public double? PackedHeight { get; set; }
        public double? PackedWidhth { get; set; }
        public double? PackedDepth { get; set; }
        public double? Weight { get; set; }
        public bool RequiresRefrigeration { get; set; }
    }

    public class ProductAdvanced
    {
        public int ProductVariantId { get; set; }
        public int BranchId { get; set; }
        public int QuantityAvailable { get; set; }

        public int MinimumStockLevel { get; set; }

        public int MaximumStockLevel { get; set; }

        public int ReorderPoint { get; set; }

        public bool BackOrders { get; set; }

    }





    public class ProductsMapOptionResult 
    {

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public List<VariantResult> Variants { get; set; }
    }


    public class VariantResult
    {
        public int VariantId { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public bool Selected { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }

    public class VariantResultTemp
    {
        public int VariantId { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public bool Selected { get; set; }
        public string Attributes { get; set; }
    }

    public class ProductParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public bool ascending { get; set; } = true;
        public string searchTerm { get; set; } = string.Empty;

    }


    public class ProductMobileParams
    {
        public int OutletId { get; set; }
        public string Category { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; } = "name"; // name, price, newest
        public string SortOrder { get; set; } = "asc"; // asc, desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }



    public class ProductPOSParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string searchTerm { get; set; } = string.Empty;

    }

    public class ProductResult
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public double CostPrice { get; set; }
        public int Count { get; set; }
		public string Thumbnail { get; set; }
		public string? Status { get; set; }

	}


    public class CustomerProductResult
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public string Thumbnail { get; set; }
        public double? Rating { get; set; }
        public int?  RatingCount { get; set; }
    }

    public class CustomerResultBaseOnCatefory
    {
        public string Category { get; set; }

        public List<CustomerProductResult> CustomerProductResults { get; set; }
    }







    public class ListOfImages
	{
		public List<string> ImageKeys { get; set; } 

	}

    public class ProductVariantDto
    {
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }

    public class ProductVariantResponse
    {
        public int VariantID { get; set; }
        public int ProductID { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
