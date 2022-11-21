using System.Security.Claims;

namespace Core.Backend.Secure.Auth;

public static class AuthExtensions
{

    public static string GetUsername(this ClaimsPrincipal cp) =>
        cp.Claims.First(x => x.Type.Contains("username")).Value;

    public static string GetUUID(this ClaimsPrincipal cp) =>
        cp.Claims.First(x => x.Type.Contains("uuid")).Value;

}
