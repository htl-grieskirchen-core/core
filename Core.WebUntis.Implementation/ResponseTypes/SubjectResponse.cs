using System.Text.Json.Serialization;
using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class SubjectResponse
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("longName")] public string? LongName { get; set; }
    [JsonPropertyName("alternativeName")] public string? AlternativeName { get; set; }
    [JsonPropertyName("active")] public bool Active { get; set; }

    public Subject Convert()
    {
        return new Subject
        {
            Id = Id,
            Name = Name,
            LongName = LongName
        };
    }
}
