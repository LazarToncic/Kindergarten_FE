using Kindergarten_FE.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kindergarten_FE.Services;

public class UserRoleService(ITokenStorageService tokenStorageService) : IUserRoleService
{
    public async Task<IEnumerable<string>> GetRolesAsync()
    {
        var token = await tokenStorageService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
            return Enumerable.Empty<string>();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        
        var roles = jwt.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value);
        
        foreach (var r in roles)
        {
            Console.WriteLine("Found role: " + r);
        }

        return jwt.Claims
            .Where(x => x.Type == ClaimTypes.Role || x.Type == "role")
            .Select(x => x.Value);
    }

    public async Task<bool> HasRoleAsync(string role)
    {
        var roles = await GetRolesAsync();
        return roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<bool> HasAnyRoleAsync()
    {
        var token = await tokenStorageService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return false;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role").Select(c => c.Value);
        return roles.Any();
    }
}