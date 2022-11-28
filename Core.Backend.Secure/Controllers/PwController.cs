using Core.AuthLib;
using Core.Backend.Secure.Dtos;
using Core.Backend.Secure.exceptions;
using Core.Backend.Secure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Backend.Secure.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PwController : ControllerBase
{
    private CredService _cred;

    public PwController(CredService cred) => _cred = cred;

    [Authorize(Policy = "Auth-Token")]
    [HttpPost("MoodleToken")]
    public ActionResult SetEduvidualToken([FromBody] string token)
    {
        try
        {
            _cred.SaveToken(User.GetUUID(), token, "eduvidual");
            return Ok();
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Policy = "Auth-Token")]
    [HttpPost("WebuntisToken")]
    public ActionResult SetWebuntisToken([FromBody] string token)
    {
        try
        {
            _cred.SaveToken(User.GetUUID(), token, "webuntis");
            return Ok();
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Policy = "Auth-Token")]
    [HttpPost("LDAP")]
    public ActionResult SaveLdapPassword([FromBody] LdapUserDto ldapUserDto)
    {
        try
        {
            _cred.SaveLdapPassword(User.GetUUID(), ldapUserDto.LdapUsername, ldapUserDto.LdapPassword);
            return Ok();
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }
}
