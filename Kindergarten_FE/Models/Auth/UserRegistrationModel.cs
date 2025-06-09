using System.ComponentModel.DataAnnotations;

namespace Kindergarten_FE.Models;

public class UserRegistrationModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required, Range(1940, 2025)]
    public int YearOfBirth { get; set; }
    [Required]
    public string Username { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    
}