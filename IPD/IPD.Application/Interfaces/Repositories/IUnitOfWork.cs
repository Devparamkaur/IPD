namespace IPD.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> Save(CancellationToken cancellationToken = default(CancellationToken));

        Task Rollback();
    }
}
