using MediatR;
using Microsoft.EntityFrameworkCore;
using Velen.Domain.SeedWork;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Domain;
using Velen.Infrastructure.Processing;

namespace Velen.Application.Behaviors;


public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest:IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _appDbContext;
    public UnitOfWorkBehavior(IUnitOfWork unitOfWork, AppDbContext appDbContext)
    {
        _unitOfWork = unitOfWork;
        _appDbContext = appDbContext;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_appDbContext.Database.CurrentTransaction!=null)
        {
            return await next();
        }
        var response = await next();
        var command = request as ICommand<TResponse>;
        if (command is InternalCommandBase)
        {
            var internalCommand = await _appDbContext.InternalCommands
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.Now;
            }
        }
        await this._unitOfWork.CommitAsync(cancellationToken);
        return response;
    }
}
