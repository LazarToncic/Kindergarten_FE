using Kindergarten_FE.Common.Interfaces;

namespace Kindergarten_FE.Services;

public class AuthStateService : IAuthStateService
{
    public event Func<Task>? OnAuthStateChanged;

    public async Task NotifyAuthChanged()
    {
        if (OnAuthStateChanged is not null)
        {
            await OnAuthStateChanged.Invoke();
        }
    }
}