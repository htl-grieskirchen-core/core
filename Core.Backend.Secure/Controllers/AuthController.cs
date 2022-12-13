using System.Text.RegularExpressions;
using Core.AuthLib;
using Core.Backend.Secure.Auth;
using Core.Backend.Secure.Services;
using Core.Database;
using Core.Ldap.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Core.Ldap.Interface.SignInResult;

namespace Core.Backend.Secure.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private JwtService _jwtService;
    private CoreContext _db;
    private ILdapClient _ldap;
    private IConfiguration _conf;

    public AuthController(JwtService jwt, CoreContext db, ILdapClient ldap, IConfiguration conf)
    {
        _jwtService = jwt;
        _db = db;
        _ldap = ldap;
        _conf = conf;
    }

    [Authorize(Policy = "Auth-Token")]
    [HttpGet]
    public string GetIdToken()
    {
        var user = _db.Users.FirstOrDefault(x => x.UUID == User.GetUUID());

        var obj = _db.StoredUserTokens.First(x => x.UserUUID == user.UUID);
        var connectedPlatforms = obj.GetType().GetProperties().Where(x => x.Name != "UserUUID" && x.Name != "User")
            .Where(x => x.GetValue(obj, null) is not null).Select(x => x.Name).ToList();

        return _jwtService.GenerateToken(new IDToken()
        {
            Username = user.Username,
            Email = user.SchoolEmail,
            Role = user.Role.ToString(),
            Class = user.Class,
            UUID = user.UUID,
            ConnectedPlatforms = connectedPlatforms,
            MatriculationNumber = user.MatriculationNumber,
        });
    }

    [HttpPost]
    public IActionResult Login(SignInParams signInParams)
    {
        SignInResult signInResult;

        try
        {
            signInResult = _ldap.SignIn(signInParams);
        }
        catch (Exception e) when (e is InvalidLoginException or LdapNotReachableException)
        {
            return BadRequest(e.Message);
        }

        var user = UpdateUserInDB(signInResult);

        DateTime valid = _conf["JWT:Auth-Token-Expiration-Unit"] == "days"
            ? DateTime.Now.AddDays(Convert.ToInt32(_conf["JWT:Auth-Token-Expiration"]))
            : DateTime.Now.AddMinutes(Convert.ToInt32(_conf["JWT:Auth-Token-Expiration"]));

        var cookie = new CookieOptions()
        {
            HttpOnly = true,
            IsEssential = true,
            Expires = valid,
            Secure = true,
            SameSite = SameSiteMode.None
        };

        var token = _jwtService.GenerateToken(new AuthToken()
        {
            Username = signInResult.User.LoginName,
            UUID = user.UUID,
        });

        Response.Cookies.Append("auth", token, cookie);
        return Ok();
    }

    [NonAction]
    public User UpdateUserInDB(SignInResult signInResult)
    {
        string? mnr = null;
        if (signInResult.User.OrganizationUnit.Equals(LdapGroup.Schueler))
        {
            var rg = new Regex(@"\d{6}");
            var mr = rg.Match(signInResult.User.LoginName);

            if (!mr.Success)
            {
                throw new Exception("Matriculation number not found");
            }

            mnr = mr.Value;
        }

        var user = _db.Users.FirstOrDefault(x => x.SchoolEmail == signInResult.User.Email);
        if (user is null)
        {
            user = _db.Users.Add(new User
            {
                SchoolEmail = signInResult.User.Email,
                StoredUserTokens = new StoredUserTokens(),
                Class = signInResult.User.Class,
                Role = signInResult.User.OrganizationUnit,
                Username = signInResult.User.LoginName,
                DisplayName = signInResult.User.DisplayName,
                MatriculationNumber = mnr
            }).Entity;
        }
        else
        {
            user.SchoolEmail = signInResult.User.Email;
            user.Class = signInResult.User.Class;
            user.Role = signInResult.User.OrganizationUnit;
            user.Username = signInResult.User.LoginName;
            user.DisplayName = signInResult.User.DisplayName;
            user.MatriculationNumber = mnr;
        }

        _db.SaveChanges();
        return user;
    }

    [Authorize]
    [HttpGet]
    public bool IsAuthed() => true;

    [Authorize(Policy = "ID-Token")]
    [HttpGet]
    public string? GetRole() => User.Claims.FirstOrDefault(x => x.Type == "rolle")?.Value;
}
