using Kindergarten_FE.Common.Interfaces;

namespace Kindergarten_FE.Services;

public class AppAuthInitializerService(ITokenStorageService tokenStorageService, ITokenRefreshService tokenRefreshService,
    IAuthStateService authState) : IAppAuthInitializerService
{
    public async Task InitializeAsync()
    {
        var accessToken = await tokenStorageService.GetAccessTokenAsync();
        var accessTokenExpiresAt = await tokenStorageService.GetAccessTokenExpiresAtAsync();

        var isAccessTokenValid = !string.IsNullOrWhiteSpace(accessToken)
                                 && accessTokenExpiresAt.HasValue
                                 && DateTime.UtcNow < accessTokenExpiresAt.Value;

        if (isAccessTokenValid)
        {
            await authState.NotifyAuthChanged();
            return;
        }

        var refreshed = await tokenRefreshService.TryRefreshTokenAsync();
        if (refreshed)
        {
            await authState.NotifyAuthChanged();
        }
    }
}