using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class OutletSearchRequest
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double RadiusKm { get; set; } = 10;
        public string Category { get; set; }
        public double? MinRating { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class OutletSeachDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Category { get; set; }
        public double? Rating { get; set; }
        public double? ReviewCount { get; set; }
        public double Distance { get; set; }
        public string Thumbnail { get; set; }
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}
