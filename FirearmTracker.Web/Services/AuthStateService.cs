using FirearmTracker.Core.Models;
using FirearmTracker.Core.Interfaces;

namespace FirearmTracker.Web.Services
{
    public class AuthStateService : IAuthStateService
    {
        private User? _currentUser;

        public event Action? OnAuthStateChanged;

        public User? CurrentUser => _currentUser;

        public bool IsAuthenticated => _currentUser != null;

        public bool IsOwner => _currentUser?.Role == UserRoles.Owner;

        public bool IsAdmin => _currentUser?.Role == UserRoles.Administrator;

        public bool IsPowerUser => _currentUser?.Role == UserRoles.PowerUser;

        public bool IsReadOnly => _currentUser?.Role == UserRoles.ReadOnly;

        // Can manage users (Owner and Administrator)
        public bool CanManageUsers => IsOwner || IsAdmin;

        // Can edit data (everyone except ReadOnly)
        public bool CanEdit => IsOwner || IsAdmin || IsPowerUser;

        // Can delete data (everyone except ReadOnly)
        public bool CanDelete => IsOwner || IsAdmin || IsPowerUser;

        // Can add data (everyone except ReadOnly)
        public bool CanAdd => IsOwner || IsAdmin || IsPowerUser;

        // Can view admin area (Owner and Administrator only)
        public bool CanAccessAdmin => IsOwner || IsAdmin;

        public void SetUser(User user)
        {
            _currentUser = user;
            OnAuthStateChanged?.Invoke();
        }

        public void ClearUser()
        {
            _currentUser = null;
            OnAuthStateChanged?.Invoke();
        }
    }
}