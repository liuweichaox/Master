using System.Text.Json;
using Master.Domain.SeedWork;
using Master.Infrastructure.Domain;
using MediatR;

namespace Master.Application.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly AppDbContext _appDbContext;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork, AppDbContext appDbContext)
    {
        _unitOfWork = unitOfWork;
        _appDbContext = appDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(@"UnitOfWorkBehavior Handle command type: " + request.GetType().Name + @"result json:" +
                          JsonSerializer.Serialize(request));
        if (_appDbContext.Database.CurrentTransaction != null)
        {
            return await next();
            Console.WriteLine(@"UnitOfWorkBehavior Handle has transaction");
        }

        var response = await next();

        await _unitOfWork.CommitAsync(cancellationToken);
        Console.WriteLine(@"UnitOfWorkBehavior Handle commit command type: " + request.GetType().Name);
        return response;
    }
}