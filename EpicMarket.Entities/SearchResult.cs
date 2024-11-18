using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class SearchResult
    {
        public List<BranchSearchDto> Branches { get; set; } = new();
        public List<ProductSearchDto> Products { get; set; } = new();
       
    }

    public class SearchRequest
    {
        public string SearchTerm { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double RadiusKm { get; set; } = 10;
    }

    public class BranchSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }
    }

    public class ProductSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public double Distance { get; set; }

    }

    public class TrendingOutletDto
    {
        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public int OrderCount { get; set; }
        public DateTime LastOrderTime { get; set; }
        public double? Distance { get; set; }
    }
}
