using System.Net.Http.Headers;
using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Dtos.Auth;
using Kindergarten_FE.Common.Interfaces;

namespace Kindergarten_FE.Services;

public class TokenRefreshService(HttpClient httpClient, ITokenStorageService tokenStorageService) : ITokenRefreshService
{
    public async Task<bool> TryRefreshTokenAsync()
    {
        var refreshToken = await tokenStorageService.GetRefreshTokenAsync();
        var refreshTokenExpiresAt = await tokenStorageService.GetRefreshTokenExpiresAtAsync();

        if (string.IsNullOrWhiteSpace(refreshToken) || !refreshTokenExpiresAt.HasValue || DateTime.UtcNow >= refreshTokenExpiresAt.Value)
            return false;

        var response = await httpClient.PostAsJsonAsync(ApiRoutes.GenerateRefreshToken, new { RefreshToken = refreshToken });

        if (!response.IsSuccessStatusCode)
            return false;

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (loginResponse is null)
            return false;

        await tokenStorageService.SaveTokensAsync(
            loginResponse.AccessToken,
            loginResponse.AccessTokenExpiresAt,
            loginResponse.RefreshToken,
            loginResponse.RefreshTokenExpiresAt,
            rememberMe: true
        );

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

        return true;
    }
}