using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IAuthStateService
    {
        event Action? OnAuthStateChanged;

        User? CurrentUser { get; }
        bool IsAuthenticated { get; }
        bool IsOwner { get; }
        bool IsAdmin { get; }
        bool IsPowerUser { get; }
        bool IsReadOnly { get; }
        bool CanManageUsers { get; }
        bool CanEdit { get; }
        bool CanDelete { get; }
        bool CanAdd { get; }
        bool CanAccessAdmin { get; }

        void SetUser(User user);

        void ClearUser();
    }
}