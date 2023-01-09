using System.Security.Claims;
using System.Text.RegularExpressions;
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
    public string Email { get; set; } = null!;

    /// <summary>
    /// Display Name
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// matrikelnummer - nur bei Schülern verfügbar
    /// </summary>
    public string? MatriculationNumber { get; set; }

    /// <summary>
    /// connectedPlatforms: Json-Arr --Plattformen mit hinterlegten credentials
    /// </summary>
    public List<string> ConnectedPlatforms { get; set; } = null!;

    public override List<Claim> Claims
    {
        get
        {

            var claims = new List<Claim>
            {
                new Claim("type", "id-token"),
                new Claim("username", Username),
                new Claim("uuid", UUID.ToString()),
                new Claim("rolle", Role),
                new Claim("email", Email),
                new Claim("connectedPlatforms", JsonConvert.SerializeObject(ConnectedPlatforms)),
            };

            if (MatriculationNumber != null)
                claims.Add(new Claim("matrikelnummer", MatriculationNumber));

            if (Class != null)
                claims.Add(new Claim("klasse", Class));

            var rg = new Regex(@"5[^b]");
            if (Class != null && rg.Match(Class).Success)
                claims.Add(new Claim("bessereKlasse", "5b"));

            return claims;
        }
    }

    public enum AvailablePlatforms
    {
        Webuntis, Eduvidual, LDAP, Sokrates, None
    }
}
