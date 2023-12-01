using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Master.Domain.SeedWork;
using Master.Infrastructure.Commands;
using Master.Infrastructure.Domain;

namespace Master.Application.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
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
        Console.WriteLine(@"UnitOfWorkBehavior Handle command type: " + request.GetType().Name + @"result json:" + JsonSerializer.Serialize(request));
        if (_appDbContext.Database.CurrentTransaction != null)
        {
            return await next();
            Console.WriteLine(@"UnitOfWorkBehavior Handle has transaction");
        }
        var response = await next();
        var command = request as ICommand<TResponse>;
        if (command is InternalCommandBase<TResponse>)
        {
            var internalCommand = await _appDbContext.InternalCommands
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.Now;
            }
        }
        await this._unitOfWork.CommitAsync(cancellationToken);
        Console.WriteLine(@"UnitOfWorkBehavior Handle commit command type: " + request.GetType().Name);
        return response;
    }
}
