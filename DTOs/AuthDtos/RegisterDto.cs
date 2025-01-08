using Dynamic_RBAMS.Models;
using System.ComponentModel.DataAnnotations;

public class RegisterDto
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Type { get; set; }
     
    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
