using System.ComponentModel.DataAnnotations;

namespace Model;

public class CreatePhishUserModel
{
    [Required]
    public string Email { get; set; }
    public string Password { get; set; }
}