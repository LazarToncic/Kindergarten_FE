using Kindergarten_FE.Models;

namespace Kindergarten_FE.Services.Interfaces;

public interface IAuthService
{
    Task<List<string>> RegisterAsync(UserRegistrationModel user);

    Task<List<string>> LoginAsync(UserLoginModel user);
}