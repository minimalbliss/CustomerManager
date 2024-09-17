using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.Api.Data.EntityFramework
{
    public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
    }
}
