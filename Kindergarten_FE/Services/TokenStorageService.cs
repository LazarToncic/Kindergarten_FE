using Kindergarten_FE.Common.Interfaces;
using Microsoft.JSInterop;

namespace Kindergarten_FE.Services;

public class TokenStorageService(IJSRuntime js) : ITokenStorageService
{
    private const string AccessTokenKey = "access_token";
    private const string AccessTokenExpiresKey = "access_token_expires";
    private const string RefreshTokenKey = "refresh_token";
    private const string RefreshTokenExpiresKey = "refresh_token_expires";
    
    public async Task SaveTokensAsync(string accessToken, DateTime accessTokenExpiresAt, string? refreshToken, DateTime? refreshTokenExpiresAt, bool rememberMe)
    {
        var storage = rememberMe ? "localStorage" : "sessionStorage";

        await js.InvokeVoidAsync($"{storage}.setItem", AccessTokenKey, accessToken);
        await js.InvokeVoidAsync($"{storage}.setItem", AccessTokenExpiresKey, accessTokenExpiresAt.ToString("O"));

        if (!string.IsNullOrEmpty(refreshToken) && refreshTokenExpiresAt.HasValue)
        {
            await js.InvokeVoidAsync($"{storage}.setItem", RefreshTokenKey, refreshToken);
            await js.InvokeVoidAsync($"{storage}.setItem", RefreshTokenExpiresKey, refreshTokenExpiresAt.Value.ToString("O"));
        }
        
        
    }

    public async Task<string?> GetAccessTokenAsync()
        => await GetFromStorageAsync(AccessTokenKey);

    public async Task<DateTime?> GetAccessTokenExpiresAtAsync()
        => ParseDateTime(await GetFromStorageAsync(AccessTokenExpiresKey));

    public async Task<string?> GetRefreshTokenAsync()
        => await GetFromStorageAsync(RefreshTokenKey);

    public async Task<DateTime?> GetRefreshTokenExpiresAtAsync()
        => ParseDateTime(await GetFromStorageAsync(RefreshTokenExpiresKey));

    public async Task ClearAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        await js.InvokeVoidAsync("localStorage.removeItem", AccessTokenExpiresKey);
        await js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
        await js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenExpiresKey);

        await js.InvokeVoidAsync("sessionStorage.removeItem", AccessTokenKey);
        await js.InvokeVoidAsync("sessionStorage.removeItem", AccessTokenExpiresKey);
        await js.InvokeVoidAsync("sessionStorage.removeItem", RefreshTokenKey);
        await js.InvokeVoidAsync("sessionStorage.removeItem", RefreshTokenExpiresKey);
    }

    private async Task<string?> GetFromStorageAsync(string key)
    {
        var local = await js.InvokeAsync<string>("localStorage.getItem", key);
        if (!string.IsNullOrEmpty(local)) return local;

        var session = await js.InvokeAsync<string>("sessionStorage.getItem", key);
        return session;
    }

    private static DateTime? ParseDateTime(string? value)
        => DateTime.TryParse(value, out var result) ? result : null;
}