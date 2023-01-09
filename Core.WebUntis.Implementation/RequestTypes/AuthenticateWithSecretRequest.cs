using System.Text.Json.Serialization;

namespace Core.WebUntis.Implementation.RequestTypes;

public class AuthenticateWithSecretRequest
{
    [JsonPropertyName("auth")] public AuthenticateWithSecretRequestAuth Auth = null!;
}

public class AuthenticateWithSecretRequestAuth
{
    [JsonPropertyName("clientTime")] public long ClientTime { get; set; }
    [JsonPropertyName("user")] public string User { get; set; } = null!;
    [JsonPropertyName("otp")] public string Otp { get; set; } = null!;
}
