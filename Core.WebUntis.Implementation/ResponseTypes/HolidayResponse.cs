using System.Text.Json.Serialization;
using HolidayInterface = Core.WebUntis.Interface.Types.Holiday;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class HolidayResponse
{
    [JsonPropertyName("active")] public ulong Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("longName")] public string? LongName { get; set; }
    [JsonPropertyName("startDate")] public int StartDateUntis { get; set; }
    [JsonPropertyName("endDate")] public int EndDateUntis { get; set; }

    public HolidayInterface Convert()
    {
        return new HolidayInterface
        {
            Id = Id,
            Name = Name,
            LongName = LongName,
            StartDate = UntisDateTimeMethods.ConvertUntisDateToDate(StartDateUntis),
            EndDate = UntisDateTimeMethods.ConvertUntisDateToDate(EndDateUntis)
        };
    }
}
