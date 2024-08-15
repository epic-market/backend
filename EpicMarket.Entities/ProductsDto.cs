using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{






	public class AddProductsDto
	{
		public int? Id { get; set; }
		public long? Barcode { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public double Rate { get; set; }
		public bool IsActive { get; set; }
		public bool InStock { get; set; }
		public bool IsRecommended { get; set; }
		public int? MaximumOrderPurchase { get; set; }
		public int? StatusId { get; set; }
		public IFormFile[] Products { get; set; }
		public IFormFile Thumbnail { get; set; }
	}

	public class ProductsDto
    {
        public int? Id { get; set; }
		public long? Barcode { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
        public string Category{ get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public bool InStock { get; set; }
        public List<string> Images { get; set; } 
		public bool IsRecommended { get; set; }
		public int? MaximumOrderPurchase { get; set; }
        public int? StatusId { get; set; }
		public string Thumbnail { get; set; }

	}
    public class ProductsMapOptionResult 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public double Rate { get; set; }

        public bool Selected { get; set; }
    }

    public class ProductParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public bool ascending { get; set; } = true;
        public string searchTerm { get; set; } = string.Empty;

    }

    public class ProductResult
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public bool InStock { get; set; }
        public int Count { get; set; }
		public string Thumbnail { get; set; }

    }
}
