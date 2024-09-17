namespace CustomerManager.Api.Data
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellation);
    }
}
