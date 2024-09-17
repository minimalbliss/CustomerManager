using CustomerApi.Api.Data.Customers.Interfaces;
using CustomerApi.Api.Data.Customers.Repositories;
using CustomerApi.Api.Data.Infrastructure;

namespace CustomerManager.Api.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private const string CategoryName = "repositorylogs";
        private readonly ApiDbContext _context;
        private readonly ILogger _logger;
        public ICustomerRepository Customers { get; private set; }

        public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(CategoryName);

            Customers = new CustomerRepository(_context, _logger);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                throw;
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
