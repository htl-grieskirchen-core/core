using System.Text.Json.Serialization;
using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class RoomResponse
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("longName")] public string? LongName { get; set; }
    [JsonPropertyName("active")] public bool? Active { get; set; }
    [JsonPropertyName("Building")] public string? Building { get; set; }

    public Room Convert()
    {
        return new Room()
        {
            Id = Id,
            Name = Name,
            LongName = LongName
        };
    }
}
