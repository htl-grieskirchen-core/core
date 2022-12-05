namespace Core.Ldap.Interface;

public class InvalidLoginException : Exception
{
    public InvalidLoginException(string? message) : base(message)
    {
    }
}
