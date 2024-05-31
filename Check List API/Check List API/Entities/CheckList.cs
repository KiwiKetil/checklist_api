namespace Check_List_API.Entities;

public readonly record struct ChecklistId(Guid checklistId)
{
    public static ChecklistId NewId => new (Guid.NewGuid());
    public static ChecklistId Empty => new (Guid.Empty);
};

public class CheckList
{

}
