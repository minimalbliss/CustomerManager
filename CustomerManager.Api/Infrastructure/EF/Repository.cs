using CustomerManager.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.Api.Infrastructure.EF
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApiDbContext _context;
        protected readonly ILogger _logger;

        public Repository(ApiDbContext context, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Repository<T>>();
            _context = context;

            if (_context is null || _context.Set<T>() is null)
            {
                _logger.LogError($"unable to initialise repository of {typeof(T)}");
                throw new NullReferenceException($"unable to initialise repository of {typeof(T)}");
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting all {typeof(T)}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting {typeof(T)} by id {id}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<T?> FindAsync(T item, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Set<T>().FindAsync(item, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while finding {typeof(T)}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<bool> AddAsync(T item, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Set<T>().AddAsync(item, cancellationToken);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding {typeof(T)}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<bool> Update(T item, CancellationToken cancellationToken)
        {
            try
            {
                _context.ChangeTracker.Clear();
                _context.Set<T>().Update(item);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating {typeof(T)}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<bool> Delete(T entity, CancellationToken cancellation)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting {typeof(T)}: {ex.Message}");
                throw;
            }
        }
    }
}
