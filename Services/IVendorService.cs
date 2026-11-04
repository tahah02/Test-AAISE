using System.Threading.Tasks;

namespace EmploymentPortal.Services
{
    public interface IVendorService
    {
        Task<bool> VerifyCandidateIdentityAsync(string email, string fullName);
    }
}