using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Dtos.Auth;
using Kindergarten_FE.Common.Interfaces;
using Kindergarten_FE.Common.Validation.Auth;
using Kindergarten_FE.Models;

namespace Kindergarten_FE.Services;

public class AuthService(HttpClient http) : IAuthService
{
    public async Task<List<string>> RegisterAsync(UserRegistrationModel user)
        => await SendPostAsync(ApiRoutes.UserRegistration, user);

    public async Task<(LoginResponseDto? Data, List<string> Errors)> LoginAsync(UserLoginModel user)
    {
        var response = await http.PostAsJsonAsync(ApiRoutes.UserLogin, new { dto = user });

        if (response.IsSuccessStatusCode)
        {
            var dto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return (dto, new List<string>());
        }

        var errorResult = await response.Content.ReadFromJsonAsync<ValidationErrorResponseDto>();
        if (errorResult?.Errors != null)
            return (null, errorResult.Errors.SelectMany(kvp => kvp.Value).ToList());

        var fallbackMessage = await response.Content.ReadAsStringAsync();
        return (null, new List<string> { fallbackMessage });
    }
    
    private async Task<List<string>> SendPostAsync<T>(string url, T payload)
    {
        var response = await http.PostAsJsonAsync(url, new { dto = payload });

        if (response.IsSuccessStatusCode)
            return new();

        var errorResult = await response.Content.ReadFromJsonAsync<ValidationErrorResponseDto>();

        if (errorResult?.Errors != null)
            return errorResult.Errors.SelectMany(kvp => kvp.Value).ToList();

        var fallbackMessage = await response.Content.ReadAsStringAsync();
        return new() { fallbackMessage };
    }
}