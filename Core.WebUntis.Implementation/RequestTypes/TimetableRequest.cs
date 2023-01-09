using System.Text.Json.Serialization;

namespace Core.WebUntis.Implementation.RequestTypes;

public class TimetableRequest
{
    public TimetableRequest(int? personId, int elementType, int startDate, int endDate)
    {
        Options = new TimetableOptions
        {
            Elements = new TimetableElement
            {
                PersonId = personId,
                ElementType = elementType
            },
            SubstitutionText = true,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    [JsonPropertyName("options")] public TimetableOptions Options { get; set; }
}

public class TimetableOptions
{
    [JsonPropertyName("element")] public TimetableElement Elements { get; set; }
    [JsonPropertyName("showSubstText")] public bool SubstitutionText { get; set; }
    [JsonPropertyName("startDate")] public int StartDate { get; set; }
    [JsonPropertyName("endDate")] public int EndDate { get; set; }
}

public class TimetableElement
{
    [JsonPropertyName("id")] public int? PersonId { get; set; }
    [JsonPropertyName("type")] public int ElementType { get; set; }
}
