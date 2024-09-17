namespace CustomerManager.Api.Data
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<T?> FindAsync(T item, CancellationToken cancellationToken);
        public Task<bool> AddAsync(T item, CancellationToken cancellationToken);
        public Task<bool> Update(T item, CancellationToken cancellationToken);
        public Task<bool> Delete(T item, CancellationToken cancellationToken);
    }
}
