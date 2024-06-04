 namespace Check_List_API.Entities;

public readonly record struct ChecklistId(Guid checklistId)
{
    public static ChecklistId NewId => new (Guid.NewGuid());
    public static ChecklistId Empty => new (Guid.Empty);
};

public class CheckList
{
    public ChecklistId Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Not Started";
    public string Priority { get; set; } = "Medium";
    public string AssignedTo { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateUpdated { get; set; } = DateTime.Now;
    public DateTime? DateCompleted { get; set; }
}
