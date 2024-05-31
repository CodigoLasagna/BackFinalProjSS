using System.ComponentModel.DataAnnotations;

namespace Domain;

public class PhishUser
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}