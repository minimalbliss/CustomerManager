using CustomerApi.Api.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.Api.Data.EntityFramework
{
    public class GenericEntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApiDbContext _context;
        internal DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public GenericEntityFrameworkRepository(ApiDbContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;

            if (_context is null || _context.Set<T>() is null)
            {
                _logger.LogError($"unable to initialise repository of {typeof(T)}");
                throw new NullReferenceException($"unable to initialise repository of {typeof(T)}");
            }

            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.ToListAsync();
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
                return await _dbSet.FindAsync(id);
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
                return await _dbSet.FindAsync(item, cancellationToken);
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
                await _dbSet.AddAsync(item, cancellationToken);
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
                _dbSet.Update(item);
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
                _dbSet.Remove(entity);
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
