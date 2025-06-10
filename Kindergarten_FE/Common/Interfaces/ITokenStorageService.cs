namespace Kindergarten_FE.Common.Interfaces;

public interface ITokenStorageService
{
    void SaveTokens(string accessToken, DateTime accessTokenExpiresAt, string? refreshToken, DateTime? refreshTokenExpiresAt, bool rememberMe);
    string? GetAccessToken();
    DateTime? GetAccessTokenExpiresAt();
    string? GetRefreshToken();
    DateTime? GetRefreshTokenExpiresAt();
    void Clear();
}