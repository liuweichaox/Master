using Velen.Domain.SeedWork;
using Velen.Infrastructure.Processing;

namespace Velen.Infrastructure.Domain
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
            await this._domainEventsDispatcher.DispatchEventsAsync();
            return await this._appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}