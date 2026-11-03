using System.Net.Http;

namespace EmploymentPortalApi.Services
{
    public class VendorService : IVendorService
    {
        private readonly HttpClient _httpClient;

        public VendorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
