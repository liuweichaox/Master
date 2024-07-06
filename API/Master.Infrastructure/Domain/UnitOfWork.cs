using Master.Domain.SeedWork;

namespace Master.Infrastructure.Domain;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(
        AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}