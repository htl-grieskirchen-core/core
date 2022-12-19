using System.ComponentModel.DataAnnotations;

namespace Core.Database;

public class StoredUserTokens
{
    [Key]
    public Guid UserUUID { get; set; }
    public string? EduvidualToken { get; set; }
    public string? LdapUsername { get; set; }
    public string? LdapPassword { get; set; }

    public User User { get; set; } = null!;
}
