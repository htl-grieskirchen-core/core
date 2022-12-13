using System.Text.RegularExpressions;
using Core.Backend.Secure.Auth;
using Core.Backend.Secure.Dtos;
using Core.Database;
using Core.Ldap.Interface;
using SignInResult = Core.Ldap.Interface.SignInResult;

namespace Core.Backend.Secure.Services;

public class AuthService
{
    private CoreContext _db;
    private ILdapClient _ldap;
    private CredService _credService;

    public AuthService(CoreContext db, ILdapClient ldap, CredService credService)
    {
        _db = db;
        _ldap = ldap;
        _credService = credService;
    }

    public SignInResult SignIn(SignInParams signInParams) => _ldap.SignIn(signInParams);

    public User GetUser(Guid uuid) => _db.Users.First(x => x.UUID == uuid);

    public List<string> GetPlatforms(User user)
    {
        var obj = _db.StoredUserTokens.First(x => x.UserUUID == user.UUID);
        return obj.GetType().GetProperties().Where(x => x.Name != "UserUUID" && x.Name != "User")
            .Where(x => x.GetValue(obj, null) is not null).Select(x => MapToEnum(x.Name).ToString()).Distinct().ToList();
    }

    private IDToken.AvailablePlatforms MapToEnum(string name)
    {
        if (name.ToLower().Contains("webuntis"))
            return IDToken.AvailablePlatforms.Webuntis;

        if (name.ToLower().Contains("eduvidual"))
            return IDToken.AvailablePlatforms.Eduvidual;

        if (name.ToUpper().Contains("LDAP"))
            return IDToken.AvailablePlatforms.LDAP;

        if (name.ToLower().Contains("sokrates"))
            return IDToken.AvailablePlatforms.Sokrates;

        return IDToken.AvailablePlatforms.None;
    }

    public User UpdateUserInDB(SignInResult signInResult, SignInParams signInParams)
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
                MatriculationNumber = mnr,
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
        _credService.SaveLdapPassword(user.UUID, new LdapUserDto { LdapPassword = signInParams.Password, LdapUsername = signInParams.Username }, true);

        return user;
    }
}
