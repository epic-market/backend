namespace EpicMarket.Entities
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryParams
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class TrendingBusinessParams
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class TrendingBusinessDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<FilterOutletDto> Businesses { get; set; }
    }

    public class FilterOutletDto
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

}