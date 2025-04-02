using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Data;

public class Authentication
{
    [Required] [MaxLength(100)] public string Login { get; set; }

    [Required]
    [MaxLength(24)]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    [MaxLength(24)]
    [MinLength(8)]
    public string PasswordConfirmation { get; set; }
}