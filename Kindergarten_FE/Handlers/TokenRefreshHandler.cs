using System.Net.Http.Json;
using Kindergarten_FE.Common.ApiRoutes;
using Kindergarten_FE.Common.Dtos.Auth;
using Kindergarten_FE.Common.Interfaces;

namespace Kindergarten_FE.Handlers;

public class TokenRefreshHandler(ITokenStorageService tokenStorageService, HttpClient httpClient) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = tokenStorageService.GetAccessToken();
        var accessTokenExpires = tokenStorageService.GetAccessTokenExpiresAt();

        if (!string.IsNullOrEmpty(accessToken) && accessTokenExpires.HasValue)
        {
            if (DateTime.UtcNow >= accessTokenExpires.Value)
            {
                // Token istekao, pokušaj refresh
                var refreshToken = tokenStorageService.GetRefreshToken();
                var refreshTokenExpires = tokenStorageService.GetRefreshTokenExpiresAt();

                if (!string.IsNullOrEmpty(refreshToken) && refreshTokenExpires > DateTime.UtcNow)
                {
                    var payload = new { refreshToken };
                    var response = await httpClient.PostAsJsonAsync(ApiRoutes.GenerateRefreshToken, payload, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var newTokens = await response.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken: cancellationToken);
                        tokenStorageService.SaveTokens(
                            newTokens!.AccessToken,
                            newTokens.AccessTokenExpiresAt,
                            newTokens.RefreshToken,
                            newTokens.RefreshTokenExpiresAt,
                            rememberMe: true // možeš ovo da menjaš ako čuvaš iz state-a
                        );

                        accessToken = newTokens.AccessToken;
                    }
                    else
                    {
                        // Refresh nije uspeo, korisnika možeš izlogovati
                        tokenStorageService.Clear();
                        return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
            }

            // Dodaj Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}