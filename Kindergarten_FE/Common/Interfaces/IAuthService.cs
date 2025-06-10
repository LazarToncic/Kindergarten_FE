using Kindergarten_FE.Common.Dtos.Auth;
using Kindergarten_FE.Models;

namespace Kindergarten_FE.Common.Interfaces;

public interface IAuthService
{
    Task<List<string>> RegisterAsync(UserRegistrationModel user);

    Task<(LoginResponseDto? Data, List<string> Errors)> LoginAsync(UserLoginModel user);
}