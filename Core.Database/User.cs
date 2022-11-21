using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UUID { get; set; }
    [Required]
    public string SchoolEmail { get; set; } = null!;

    public StoredUserTokens? StoredUserTokens { get; set; }
}
