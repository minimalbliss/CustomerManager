using CustomerManager.Api.Data;
using System.Collections;

namespace CustomerManager.Api.Infrastructure.EF
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private const string CategoryName = "repositorylogs";
        private readonly ApiDbContext _dbContext;
        private Hashtable _repositories;
        private readonly ILogger _logger;
        private readonly ICustomerRepository _customerRepository;


        public UnitOfWork(ApiDbContext dbContext, ICustomerRepository customerRepository, ILoggerFactory loggerFactoryy)
        {
            _dbContext = dbContext;
            _logger = loggerFactoryy.CreateLogger(CategoryName);
            _customerRepository = customerRepository;
        }
        public ICustomerRepository CustomerRepository { get { return _customerRepository; } }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = [];

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                throw;
            }
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
