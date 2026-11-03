using Microsoft.EntityFrameworkCore;

namespace EmploymentPortalApi.Data
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
            : base(options)
        {
        }
    }
}
