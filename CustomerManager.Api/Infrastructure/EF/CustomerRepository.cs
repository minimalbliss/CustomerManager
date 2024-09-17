using CustomerManager.Api.Data;
using CustomerManager.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.Api.Infrastructure.EF
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly ApiDbContext context;
        private readonly ILogger<CustomerRepository> logger;

        public CustomerRepository(ApiDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<CustomerRepository>();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetByNameAsync(string name)
        {
            return await context.Customers.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
