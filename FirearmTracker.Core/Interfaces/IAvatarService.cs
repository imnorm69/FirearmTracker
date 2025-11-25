using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirearmTracker.Core.Interfaces
{
    public interface IAvatarService
    {
        Task<(byte[]? imageData, string? contentType)> GetUserAvatarAsync(int userId);
    }
}