using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Ldap.Interface;

namespace Core.Database;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UUID { get; set; }
    [Required]
    public string SchoolEmail { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    public string? Class { get; set; }

    [Required]
    public LdapGroup Role { get; set; }

    [Required]
    public string DisplayName { get; set; } = null!;

    public string? MatriculationNumber { get; set; }

    public StoredUserTokens? StoredUserTokens { get; set; }
}
