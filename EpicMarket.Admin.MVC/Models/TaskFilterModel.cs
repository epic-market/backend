public class TaskFilterModel
{
    public string TaskId { get; set; }
    public string TaskName { get; set; }
    public string AssignedTo { get; set; }
    public int? StatusId { get; set; }
    public int? TypeId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "id";
    public string SortDirection { get; set; } = "asc";
} 