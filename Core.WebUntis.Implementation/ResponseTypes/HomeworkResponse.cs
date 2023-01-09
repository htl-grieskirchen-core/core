using System.Text.Json.Serialization;
using HomeworkInterface = Core.WebUntis.Interface.Types.Homework;

namespace Core.WebUntis.Implementation.ResponseTypes;

class HomeworkResponse
{
    [JsonPropertyName("data")] public DataHomeWorkResponse? Data { get; set; }

    public IEnumerable<HomeworkInterface> Convert()
    {
        return Data.Homeworks.Select(x =>
        {
            x.Convert();
            return new HomeworkInterface
            {
                Id = x.Id,
                LessonId = x.LessonId,
                TeacherId = Data.Records.First(y => y.HomeworkId == x.Id).TeacherId,
                Date = x.Date,
                DueDate = x.DueDate,
                Completed = x.Completed,
                Text = x.Text,
                Remark = x.Remark,
                Attachments = x.Attachments
            };
        });
    }
}

internal class RecordHomeWorkResponse
{
    [JsonPropertyName("homeworkId")] public long HomeworkId { get; set; }
    [JsonPropertyName("teacherId")] public long TeacherId { get; set; }
    [JsonPropertyName("elementIds")] public List<ulong>? ElementIds { get; set; }
}

internal class TeacherHomeWorkResponse
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

internal class DataHomeWorkResponse
{
    [JsonPropertyName("records")] public List<RecordHomeWorkResponse>? Records { get; set; }
    [JsonPropertyName("homeworks")] public List<Homework>? Homeworks { get; set; }
    [JsonPropertyName("teachers")] public List<TeacherHomeWorkResponse>? Teachers { get; set; }
    [JsonPropertyName("lessons")] public List<LessonHomeWork>? Lessons { get; set; }
}

internal class Homework
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("lessonId")] public long LessonId { get; set; }
    [JsonPropertyName("date")] public int DateUntis { get; set; }
    [JsonPropertyName("dueDate")] public int DueDateUntis { get; set; }
    [JsonPropertyName("text")] public string? Text { get; set; }
    [JsonPropertyName("remark")] public string? Remark { get; set; }
    [JsonPropertyName("completed")] public bool Completed { get; set; }
    [JsonPropertyName("attachments")] public string[]? Attachments { get; set; }

    public DateTime Date { get; private set; }
    public DateTime DueDate { get; private set; }

    public void Convert()
    {
        Date = UntisDateTimeMethods.ConvertUntisDateToDate(DateUntis);
        DueDate = UntisDateTimeMethods.ConvertUntisDateToDate(DueDateUntis);
    }
}

internal class LessonHomeWork
{
    [JsonPropertyName("id")] public ulong Id { get; set; }

    [JsonPropertyName("subject")] public string? Subject { get; set; }

    [JsonPropertyName("lessonType")] public string? LessonType { get; set; }
}
