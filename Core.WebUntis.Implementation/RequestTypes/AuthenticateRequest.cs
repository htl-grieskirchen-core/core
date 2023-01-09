using System.Text.Json.Serialization;

namespace Core.WebUntis.Implementation.RequestTypes;

public class AuthenticateRequest
{
    [JsonPropertyName("user")] public string User { get; set; } = null!;
    [JsonPropertyName("password")] public string Password { get; set; } = null!;
    [JsonPropertyName("client")] public string Client { get; set; } = null!;
}
