using Dynamic_RBAMS.Models;
using System.ComponentModel.DataAnnotations;

public class RegisterBaseDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required]
    public string Role { get; set; }

    [Required]
    public virtual string Type { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string Address { get; set; }

    public string Gender { get; set; }

    public string EmergencyContact { get; set; }
}

