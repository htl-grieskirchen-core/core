using System.Text.Json.Serialization;
using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class ClassResponse
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("longName")] public string? LongName { get; set; }
    [JsonPropertyName("active")] public bool? Active { get; set; }
    [JsonPropertyName("teacher1")] public int? Teacher1 { get; set; }
    [JsonPropertyName("did")] public int? Did { get; set; }

    public Class Convert()
    {
        return new Class
        {
            Id = Id,
            Name = Name,
            LongName = LongName
        };
    }
}
