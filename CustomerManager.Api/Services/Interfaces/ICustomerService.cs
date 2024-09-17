using CustomerManager.Models.Models;
using LanguageExt.Common;

namespace CustomerManager.Api.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToke);
        public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Customer?> FindAsync(Customer item, CancellationToken cancellationToken);
        public Task<Result<bool>> AddAsync(Customer item, CancellationToken cancellationToken);
        public Task<Result<bool>> UpdateAsync(Customer item, CancellationToken cancellationToken);
        public Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken);

    }
}