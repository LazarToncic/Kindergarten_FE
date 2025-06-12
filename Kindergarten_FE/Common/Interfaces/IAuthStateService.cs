namespace Kindergarten_FE.Common.Interfaces;

public interface IAuthStateService
{
   Task NotifyAuthChanged();
   event Func<Task>? OnAuthStateChanged;
}