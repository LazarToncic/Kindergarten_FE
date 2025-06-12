using System.ComponentModel.DataAnnotations;

namespace Kindergarten_FE.Models;

public class UserRegistrationModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required, Range(1940, 2025)]
    public int YearOfBirth { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
    
}