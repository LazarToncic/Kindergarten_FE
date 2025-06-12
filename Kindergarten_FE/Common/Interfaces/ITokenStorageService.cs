namespace Kindergarten_FE.Common.Interfaces;

public interface ITokenStorageService
{
    Task SaveTokensAsync(string accessToken, DateTime accessTokenExpiresAt, string? refreshToken, DateTime? refreshTokenExpiresAt, bool rememberMe);
    Task<string?> GetAccessTokenAsync();
    Task<DateTime?> GetAccessTokenExpiresAtAsync();
    Task<string?> GetRefreshTokenAsync();
    Task<DateTime?> GetRefreshTokenExpiresAtAsync();
    Task ClearAsync();
}