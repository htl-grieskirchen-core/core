using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Core.Backend.Secure.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Core.Backend.Secure.Services;

public class JwtService
{
    private IConfiguration _conf;
    private RSA _keys;

    public JwtService(IConfiguration conf, RSA keys)
    {
        _conf = conf;
        _keys = keys;
    }

    public string GenerateToken(AuthToken user)
    {
        SecurityKey key = new RsaSecurityKey(_keys);

        DateTime expiry;
        if (user.GetType() == typeof(AuthToken))
        {
            expiry = _conf["JWT:Auth-Token-Expiration-Unit"] == "days"
                ? DateTime.Now.AddDays(Convert.ToInt32(_conf["JWT:Auth-Token-Expiration"]))
                : DateTime.Now.AddMinutes(Convert.ToInt32(_conf["JWT:Auth-Token-Expiration"]));
        }
        else
        {
            expiry = _conf["JWT:ID-Token-Expiration-Unit"] == "days"
                ? DateTime.Now.AddDays(Convert.ToInt32(_conf["JWT:ID-Token-Expiration"]))
                : DateTime.Now.AddMinutes(Convert.ToInt32(_conf["JWT:ID-Token-Expiration"]));
        }

        var token = new JwtSecurityToken(
            issuer: _conf["JWT:Issuer"],
            audience: _conf["JWT:Audience"],
            claims: user.Claims,
            expires: expiry,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
