namespace Kindergarten_FE.Common.Interfaces;

public interface IUserRoleService
{
    IEnumerable<string> GetRoles();
    bool HasRole(string role);
}