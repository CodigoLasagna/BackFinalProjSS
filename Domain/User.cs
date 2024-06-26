using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? rsa_id { get; set; }
}