using System.Security.Claims;
using Newtonsoft.Json;

namespace Core.Backend.Secure.Auth;

public class IDToken : AuthToken
{
    /// <summary>
    /// rolle: <schüler, lehrer, staff>
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// klasse - nur bei Schülern verfügbar
    /// </summary>
    public string? Class { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// matrikelnummer - nur bei Schülern verfügbar
    /// </summary>
    public string? MatriculationNumber { get; set; }

    /// <summary>
    /// connectedPlatforms: Json-Arr --Plattformen mit hinterlegten credentials
    /// </summary>
    public List<string> ConnectedPlatforms { get; set; } = null!;

    public override Claim[] Claims
    {
        get
        {
            var claims = new[]
            {
                new Claim("type", "id-token"),
                new Claim("username", Username),
                new Claim("uuid", UUID.ToString()),
                new Claim("rolle", Role),
                new Claim("email", Email),
                new Claim("connectedPlatforms", JsonConvert.SerializeObject(ConnectedPlatforms)),
            };

            if (MatriculationNumber != null)
                claims.Append(new Claim("matrikelnummer", MatriculationNumber));

            if (Class != null)
                claims.Append(new Claim("klasse", Class));

            return claims;
        }
    }
}
