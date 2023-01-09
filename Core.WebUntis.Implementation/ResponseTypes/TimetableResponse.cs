using System.Text.Json.Serialization;
using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class TimetableResponse
{
    [JsonPropertyName("start")] public DateTime Start { get; set; }
    [JsonPropertyName("end")] public DateTime End { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("classIds")] public List<int>? ClassIds { get; set; }
    [JsonPropertyName("roomIds")] public List<int>? RoomIds { get; set; }
    [JsonPropertyName("subjectIds")] public List<int>? SubjectIds { get; set; }
    [JsonPropertyName("teacherIds")] public List<int>? TeacherIds { get; set; }
    [JsonPropertyName("lessonType")] public string? LessonType { get; set; }
    [JsonPropertyName("text")] public string? Text { get; internal set; }
    [JsonPropertyName("statisticalFlags")] public string? StatisticalFlags { get; set; }
    [JsonPropertyName("code")] public string? Code { get; internal set; }
    [JsonPropertyName("substText")] public string? SubstitutionText { get; internal set; }

    public Timetable Convert()
    {
        return new Timetable
        {
            Id = Id,
            Code = Enum.Parse<Code>(Code),
            Start = Start,
            End = End,
            Text = Text,
            ClassIds = ClassIds,
            LessonType = Enum.Parse<LessonType>(LessonType),
            RoomIds = RoomIds,
            StatisticalFlags = StatisticalFlags,
            SubjectIds = SubjectIds,
            TeacherIds = TeacherIds,
            SubstitutionText = SubstitutionText
        };
    }
}
