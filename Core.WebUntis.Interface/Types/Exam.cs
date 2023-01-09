namespace Core.WebUntis.Interface.Types;

public class Exam
{
    public int Id { get; set; }
    public List<int>? Classes { get; set; }
    public List<int>? Teachers { get; set; }
    public List<int>? Students { get; set; }
    public DateTime Date { get; set; }
    public int Subject { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
