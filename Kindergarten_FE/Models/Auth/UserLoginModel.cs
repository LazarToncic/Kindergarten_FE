using System.ComponentModel.DataAnnotations;

namespace Kindergarten_FE.Models;

public class UserLoginModel
{
    [Required(ErrorMessage = "Please enter your email or username")]
    public string EmailOrUsername { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; } = string.Empty;
}