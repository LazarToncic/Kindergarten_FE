namespace Kindergarten_FE.Common.Interfaces;

public interface IUserRoleService
{
    Task<IEnumerable<string>> GetRolesAsync();
    Task<bool> HasRoleAsync(string role);

    Task<bool> HasAnyRoleAsync();
}