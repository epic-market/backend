public class TaskViewModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TaskTypeName { get; set; }
    public string TaskStatusName { get; set; }
    public int? ParentID { get; set; }
    public int? TaskPriorityID { get; set; }
    public string PriorityText { get; set; }
    public string AssignedToName { get; set; }
    public int? AssignedToId { get; set; }
    public DateTime? DateAssigned { get; set; }
    public DateTime? DateDue { get; set; }
    public int? SubmittedByPersonID { get; set; }
    public bool IsOverdue { get; set; }
} 