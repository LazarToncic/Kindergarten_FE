namespace Kindergarten_FE.Common.Interfaces;

public interface ITokenRefreshService
{
    Task<bool> TryRefreshTokenAsync();
}