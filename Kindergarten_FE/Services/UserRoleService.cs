using Kindergarten_FE.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kindergarten_FE.Services;

public class UserRoleService(ITokenStorageService tokenStorageService) : IUserRoleService
{
    public IEnumerable<string> GetRoles()
    {
        var token = tokenStorageService.GetAccessToken();
        if (string.IsNullOrEmpty(token))
            return Enumerable.Empty<string>();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims
            .Where(x => x.Type == ClaimTypes.Role || x.Type == "role")
            .Select(x => x.Value);
    }

    public bool HasRole(string role)
    {
        return GetRoles().Contains(role, StringComparer.OrdinalIgnoreCase);
    }
}