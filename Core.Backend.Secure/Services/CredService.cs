using System.Security.Cryptography;
using System.Text;
using Core.Backend.Secure.Dtos;
using Core.Backend.Secure.exceptions;
using Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Core.Backend.Secure.Services;

public class CredService
{
    private CoreContext _db;
    private RSA _rsa;
    private IConfiguration _conf;

    public CredService(CoreContext db, IConfiguration conf, RSA rsa)
    {
        _db = db;
        _conf = conf;
        _rsa = rsa;
        //_privateKey = RsaService.ImportRSAKey("./keys/" + _conf["RSA:private-key"]);
        //_publicKey = ImportPublicKey("./keys/"+_conf["RSA:public-key"]);
    }

    public string? GetLdapPassword(Guid uuid) => DecryptPw(GetUserTokenSet(uuid).LdapPassword);
    public string? GetEduvidualToken(Guid uuid) => DecryptPw(GetUserTokenSet(uuid).EduvidualToken);
    private StoredUserTokens GetUserTokenSet(Guid uuid) => _db.StoredUserTokens.Include(x => x.User).First(x => x.UserUUID == uuid);
    private string? DecryptPw(string? pw) => pw is not null ? Encoding.UTF8.GetString(_rsa.Decrypt(Convert.FromBase64String(pw), RSAEncryptionPadding.OaepSHA512)) : pw;
    private string EncryptPw(string pw) => Convert.ToBase64String(_rsa.Encrypt(Encoding.UTF8.GetBytes(pw), RSAEncryptionPadding.OaepSHA512));

    public void SaveToken(Guid uuid, string token, string type)
    {
        var userTokens = GetUserTokenSet(uuid);
        switch (type)
        {
            case "eduvidual":
                userTokens.EduvidualToken = token;
                _db.SaveChanges();
                break;
            default:
                throw new InvalidTypeException($"Type {type} is not a valid type");
        }
    }

    public void SaveLdapPassword(Guid uuid, LdapUserDto user, bool encrypt = false)
    {
        var userTokens = GetUserTokenSet(uuid);
        userTokens.LdapUsername = user.LdapUsername;
        userTokens.LdapPassword = encrypt ? EncryptPw(user.LdapPassword) : user.LdapPassword;
        _db.SaveChanges();
    }
}
