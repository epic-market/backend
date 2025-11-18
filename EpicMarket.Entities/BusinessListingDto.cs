using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    /// <summary>
    /// Represents a single business in listings
    /// </summary>
    public class BusinessListingDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Badge { get; set; }
        public string WaitTime { get; set; }
        public List<string> Features { get; set; }
    }

    /// <summary>
    /// Represents a group of businesses (e.g., Trending, New, Featured)
    /// </summary>
    public class BusinessGroupDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<BusinessListingDto> Businesses { get; set; }
    }

    /// <summary>
    /// Response containing multiple business groups
    /// </summary>
    public class BusinessGroupsResponseDto
    {
        public List<BusinessGroupDto> BusinessGroups { get; set; }
    }
} 