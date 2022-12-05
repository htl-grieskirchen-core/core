namespace Core.WebUntis.Interface.Types;

public class Student
{
    public int? ClassId { get; set; }
    public int? ClassTeacherId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
}
