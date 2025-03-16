public class SupportQueryFilterModel
{
    public string Query { get; set; }
    public int? PersonTypeId { get; set; }
    public int? TaskTypeId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "id";
    public string SortDirection { get; set; } = "asc";
} 