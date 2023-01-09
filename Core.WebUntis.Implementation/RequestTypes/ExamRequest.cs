using System.Text.Json.Serialization;

namespace Core.WebUntis.Implementation.RequestTypes;

public class ExamRequest
{
    [JsonPropertyName("examTypeId")] public int ExamTypeId { get; set; }
    [JsonPropertyName("startDate")] public int StartDate { get; set; }
    [JsonPropertyName("endDate")] public int EndDate { get; set; }
}
