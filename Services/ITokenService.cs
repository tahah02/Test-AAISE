using EmploymentPortal.Models;

namespace EmploymentPortal.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}