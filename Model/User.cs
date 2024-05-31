using System.ComponentModel.DataAnnotations;

namespace Model;

public class CreateUserModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class UpdateUserModel
{
    [StringLength(maximumLength: 200, MinimumLength = 0)]
    public string? Name { get; set; }
    [StringLength(maximumLength: 200, MinimumLength = 0)]
    public string? Email { get; set; }
    [StringLength(maximumLength: 200, MinimumLength = 0)]
    public string? Password { get; set; }
}
public class LoginUserModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
