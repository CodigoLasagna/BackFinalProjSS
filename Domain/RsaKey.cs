using System.ComponentModel.DataAnnotations;

namespace Domain;

public class RsaKey
{
    [Key]
    public int Id { get; set; }
    // public part is n and e
    // private part is n and d
    public string Npart { get; set; }
    public string Epart { get; set; }
    public string Dpart { get; set; }
}