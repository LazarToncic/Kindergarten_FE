using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Dtos.Auth;
using Kindergarten_FE.Common.Interfaces;

namespace Kindergarten_FE.Handlers;

public class TokenRefreshHandler(ITokenStorageService tokenStorageService) : DelegatingHandler
{
    private static readonly string[] AnonymousPaths =
    {
        "auth/userregistration",
        "auth/userlogin",
        "auth/generaterefreshtoken"
    };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uriPath = request.RequestUri?.AbsolutePath.ToLowerInvariant().Trim('/');
        var isAnonymous = uriPath != null && AnonymousPaths.Any(path => uriPath.EndsWith(path));

        var accessToken = await tokenStorageService.GetAccessTokenAsync();
        var accessTokenExpires = await tokenStorageService.GetAccessTokenExpiresAtAsync();

        if (!string.IsNullOrEmpty(accessToken) && accessTokenExpires.HasValue && DateTime.UtcNow < accessTokenExpires.Value)
        {
            // Token još važi – postavi Authorization header ako NIJE za anonimnu rutu
            if (!isAnonymous)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        else
        {
            // Access token je istekao – pokušaj refresh
            var refreshToken = await tokenStorageService.GetRefreshTokenAsync();
            var refreshTokenExpires = await tokenStorageService.GetRefreshTokenExpiresAtAsync();

            if (!string.IsNullOrEmpty(refreshToken) && refreshTokenExpires > DateTime.UtcNow)
            {
                var refreshClient = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:44309/")
                };

                var payload = new { refreshToken };
                var response = await refreshClient.PostAsJsonAsync("Auth/GenerateRefreshToken", payload, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var newTokens = await response.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken: cancellationToken);

                    await tokenStorageService.SaveTokensAsync(
                        newTokens!.AccessToken,
                        newTokens.AccessTokenExpiresAt,
                        newTokens.RefreshToken,
                        newTokens.RefreshTokenExpiresAt,
                        rememberMe: true
                    );

                    if (!isAnonymous)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.AccessToken);
                    }
                }
                else
                {
                    await tokenStorageService.ClearAsync();
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}