using Kindergarten_FE.Common.Interfaces;
using Microsoft.JSInterop;

namespace Kindergarten_FE.Services;

public class TokenStorageService(IJSRuntime js) : ITokenStorageService
{
    private const string AccessTokenKey = "access_token";
    private const string AccessTokenExpiresKey = "access_token_expires";
    private const string RefreshTokenKey = "refresh_token";
    private const string RefreshTokenExpiresKey = "refresh_token_expires";
    
    public void SaveTokens(string accessToken, DateTime accessTokenExpiresAt, string? refreshToken, DateTime? refreshTokenExpiresAt, bool rememberMe)
    {
        var storage = rememberMe ? "localStorage" : "sessionStorage";

        js.InvokeVoidAsync($"{storage}.setItem", AccessTokenKey, accessToken);
        js.InvokeVoidAsync($"{storage}.setItem", AccessTokenExpiresKey, accessTokenExpiresAt.ToString("O"));

        if (!string.IsNullOrEmpty(refreshToken) && refreshTokenExpiresAt.HasValue)
        {
            js.InvokeVoidAsync($"{storage}.setItem", RefreshTokenKey, refreshToken);
            js.InvokeVoidAsync($"{storage}.setItem", RefreshTokenExpiresKey, refreshTokenExpiresAt.Value.ToString("O"));
        }
    }

    public string? GetAccessToken()
        => GetFromStorage(AccessTokenKey);

    public DateTime? GetAccessTokenExpiresAt()
        => ParseDateTime(GetFromStorage(AccessTokenExpiresKey));

    public string? GetRefreshToken()
        => GetFromStorage(RefreshTokenKey);

    public DateTime? GetRefreshTokenExpiresAt()
        => ParseDateTime(GetFromStorage(RefreshTokenExpiresKey));

    public void Clear()
    {
        js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        js.InvokeVoidAsync("localStorage.removeItem", AccessTokenExpiresKey);
        js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
        js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenExpiresKey);

        js.InvokeVoidAsync("sessionStorage.removeItem", AccessTokenKey);
        js.InvokeVoidAsync("sessionStorage.removeItem", AccessTokenExpiresKey);
        js.InvokeVoidAsync("sessionStorage.removeItem", RefreshTokenKey);
        js.InvokeVoidAsync("sessionStorage.removeItem", RefreshTokenExpiresKey);
    }

    private string? GetFromStorage(string key)
    {
        var local = js.InvokeAsync<string>("localStorage.getItem", key).Result;
        if (!string.IsNullOrEmpty(local)) return local;

        var session = js.InvokeAsync<string>("sessionStorage.getItem", key).Result;
        return session;
    }

    private DateTime? ParseDateTime(string? value)
    {
        return DateTime.TryParse(value, out var result) ? result : null;
    }
}