namespace Core.Backend.Secure.Dtos;

public class UserDTO
{

    public Guid UUID { get; set; }

    public string SchoolEmail { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Class { get; set; }

    public string Role { get; set; }

    public string DisplayName { get; set; } = null!;

    public string? MatriculationNumber { get; set; }

}
