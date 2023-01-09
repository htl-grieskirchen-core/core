using System.Text.Json.Serialization;
using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Implementation.ResponseTypes;

public class AuthenticateResponse
{
    [JsonPropertyName("sessionId")] public string SessionId { get; set; } = null!;
    [JsonPropertyName("personType")] public long PersonType { get; set; }
    [JsonPropertyName("personId")] public long PersonId { get; set; }
    [JsonPropertyName("klasseId")] public long KlasseId { get; set; }

    public Authentication Convert()
    {
        return new Authentication
        {
            Token = SessionId
        };
    }
}
