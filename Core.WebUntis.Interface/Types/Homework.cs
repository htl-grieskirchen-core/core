namespace Core.WebUntis.Interface.Types;

public class Homework
{
    public long Id { get; set; }
    public long LessonId { get; set; }
    public long TeacherId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public string? Text { get; set; }
    public string? Remark { get; set; }
    public bool Completed { get; set; }
    public string[]? Attachments { get; set; }
}
