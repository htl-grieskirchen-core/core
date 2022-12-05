namespace Core.Ldap.Interface;

public class LdapNotReachableException : Exception
{
    public LdapNotReachableException(string? message) : base(message)
    {
    }
}
