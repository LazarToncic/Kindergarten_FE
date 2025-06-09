using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Validation.Auth;
using Kindergarten_FE.Models;
using Kindergarten_FE.Services.Interfaces;

namespace Kindergarten_FE.Services;

public class AuthService(HttpClient http) : IAuthService
{
    public async Task<List<string>> RegisterAsync(UserRegistrationModel user)
        => await SendPostAsync(ApiRoutes.UserRegistration, user);

    public async Task<List<string>> LoginAsync(UserLoginModel user)
        => await SendPostAsync(ApiRoutes.UserLogin, user);
    
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