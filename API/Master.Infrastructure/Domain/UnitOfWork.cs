using Master.Domain.SeedWork;
using Master.Infrastructure.Processing;

namespace Master.Infrastructure.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(
            AppDbContext appDbContext,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this._appDbContext = appDbContext;
            this._domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            return await this._appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}