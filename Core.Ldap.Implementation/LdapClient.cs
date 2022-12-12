using System.Collections;
using Core.Ldap.Interface;
using System.DirectoryServices.Protocols;
using System.Net;
using Microsoft.Extensions.Options;

namespace Core.Ldap.Implementation;

public class LdapClient : ILdapClient
{
    private readonly LdapConfiguration _ldapConfiguration;

    public LdapClient(IOptions<LdapConfiguration> ldapConfiguration) => _ldapConfiguration = ldapConfiguration.Value;

    public SignInResult SignIn(SignInParams signInParams)
    {
        var credentials = new NetworkCredential(signInParams.Username, signInParams.Password, "HTL");
        var serverId = new LdapDirectoryIdentifier(_ldapConfiguration.ServerIp);
        var connection = new LdapConnection(serverId, credentials)
        {
            AuthType = AuthType.Basic,
            SessionOptions =
            {
                ReferralChasing = ReferralChasingOptions.None
            },
            Timeout = TimeSpan.FromSeconds(5)
        };
        try
        {
            connection.Bind();
            return new SignInResult { User = GetLdapUser(signInParams, connection) };
        }
        catch (LdapException ex)
        {
            if (ex.Message == "The supplied credential is invalid.")
            {
                throw new InvalidLoginException("Hey Schüler/Lehrer! Deine Anmeldedaten sind falsch!");
            }

            if (ex.Message == "The LDAP server is unavailable.")
            {
                throw new LdapNotReachableException("Der LDAP Server ist derzeit nicht erreichbar! Bitte versuche es später noch einmal!");
            }

            throw;
        }
    }

    private static LdapUser GetLdapUser(SignInParams signInParams, DirectoryConnection connection)
    {
        var directoryEntry = GetDirectoryEntry(connection, signInParams.Username);
        var attributes = GetAllUserAttributes(directoryEntry);

        var attendingClass = attributes["memberof"].FirstOrDefault(x => x.Contains("Klasse"))?.Split("=")[1].Split(",")[0].Trim();
        var organizationUnit = GetOrganizationUnitOfString(attributes["memberof"].Where(x => x.Contains("OU")).ToArray());

        return new LdapUser
        {
            DisplayName = attributes["displayname"][0],
            LoginName = attributes["samaccountname"][0],
            Email = attributes["mail"][0],
            Class = attendingClass ?? null,
            OrganizationUnit = organizationUnit,
        };
    }

    private static LdapGroup GetOrganizationUnitOfString(string[] organizationUnits)
    {
        if (organizationUnits.Any(x => x.Contains("Administration")))
            return LdapGroup.Administrator;
        return organizationUnits.Any(x => x.Contains("Lehrer")) ? LdapGroup.Lehrer : LdapGroup.Schueler;
    }

    private static SearchResultEntryCollection GetDirectoryEntry(DirectoryConnection connection, string username)
    {
        var request = new SearchRequest("DC=htl,DC=grieskirchen,DC=local",
            $"(&(objectClass=user)(sAMAccountName={username}))",
            SearchScope.Subtree, null);
        var response = (SearchResponse)connection.SendRequest(request);
        return response.Entries;
    }

    private static Dictionary<string, string[]> GetAllUserAttributes(IEnumerable directoryEntry)
    {
        return (from SearchResultEntry o in directoryEntry
                from DictionaryEntry oAttribute in o.Attributes
                select oAttribute).ToDictionary(
                oAttribute => oAttribute.Key.ToString() ?? "No name set",
                oAttribute => ((DirectoryAttribute)oAttribute.Value!).GetValues(typeof(string))
                    .Select(x => x.ToString()).ToArray()
            )!;
    }
}
