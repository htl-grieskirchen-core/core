using System.Text.Json.Serialization;
using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class ExamResponse
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("classes")] public List<int>? Classes { get; set; }
    [JsonPropertyName("teachers")] public List<int>? Teachers { get; set; }
    [JsonPropertyName("students")] public List<int>? Students { get; set; }
    [JsonPropertyName("date")] public int Date { get; set; }
    [JsonPropertyName("subject")] public int Subject { get; set; }
    [JsonPropertyName("startTime")] public int StartTime { get; set; }
    [JsonPropertyName("endTime")] public int EndTime { get; set; }

    public Exam Convert()
    {
        return new Exam()
        {
            Id = Id,
            Classes = Classes,
            Teachers = Teachers,
            Students = Students,
            Date = UntisDateTimeMethods.ConvertUntisDateToDate(Date),
            Subject = Subject,
            StartTime = UntisDateTimeMethods.ConvertUntisTimeToTime(StartTime),
            EndTime = UntisDateTimeMethods.ConvertUntisTimeToTime(EndTime)
        };
    }
}
