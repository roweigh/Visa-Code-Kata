using Microsoft.EntityFrameworkCore;

namespace JobApplicationApi.Models
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; }
    }
}
