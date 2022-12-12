using System.Text.RegularExpressions;
using Core.Database;
using Core.Ldap.Interface;
using SignInResult = Core.Ldap.Interface.SignInResult;

namespace Core.Backend.Secure.Services;

public class AuthService
{
    private CoreContext _db;
    private ILdapClient _ldap;

    public AuthService(CoreContext db, ILdapClient ldap)
    {
        _db = db;
        _ldap = ldap;
    }

    public SignInResult SignIn(SignInParams signInParams) => _ldap.SignIn(signInParams);

    public User GetUser(Guid uuid) => _db.Users.First(x => x.UUID == uuid);

    public List<string> GetPlatforms(User user)
    {
        var obj = _db.StoredUserTokens.First(x => x.UserUUID == user.UUID);
        return obj.GetType().GetProperties().Where(x => x.Name != "UserUUID" && x.Name != "User")
            .Where(x => x.GetValue(obj, null) is not null).Select(x => x.Name).ToList();
    }

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
}
