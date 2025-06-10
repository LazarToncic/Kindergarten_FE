namespace Kindergarten_FE.Common.Dtos.Auth;

public record LoginResponseDto(string AccessToken,
    DateTime AccessTokenExpiresAt,
    string? RefreshToken,
    DateTime? RefreshTokenExpiresAt);