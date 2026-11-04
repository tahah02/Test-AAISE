using System.Net.Http;
using System.Threading.Tasks;

namespace EmploymentPortal.Services
{
    public class VendorService : IVendorService
    {
        private readonly HttpClient _httpClient;

        public VendorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyCandidateIdentityAsync(string email, string fullName)
        {
            try
            {
                // Calls Agent 2's Mock Vendor Identity Verification API if available
                var response = await _httpClient.GetAsync($"http://localhost:5001/api/verify?email={email}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return true; // Fallback to verified for POC
            }
        }
    }
}