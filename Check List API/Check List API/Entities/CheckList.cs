namespace Check_List_API.Entities;

public class CheckList
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } = "Not Started";
    public string Priority { get; set; } = "Medium";
    public DateTime? DueDate { get; set; }
    public string AssignedTo { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateCompleted { get; set; }
    public string Comments { get; set; }
}
//"title": "Submit project report",
//  "description": "Compile all sections and submit the final project report.",
//  "status": "In Progress",
//  "priority": "High",
//  "due_date": "2024-06-15",
//  "assigned_to": "John Doe",
//  "date_created": "2024-05-01",
//  "date_completed": null,