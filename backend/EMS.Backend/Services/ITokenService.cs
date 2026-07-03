using EMS.Backend.Models;

namespace EMS.Backend.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user, out DateTime expiresAt);
    }
}
