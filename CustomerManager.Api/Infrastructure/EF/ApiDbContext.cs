using CustomerManager.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.Api.Infrastructure.EF
{
    public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
    }
}
