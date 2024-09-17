using CustomerManager.Models.Models;

namespace CustomerManager.Api.Data
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByNameAsync(string nameme);
        Task<Customer?> GetByEmailAsync(string email);
    }
}
