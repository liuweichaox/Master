using MediatR;
using Master.Infrastructure.Queries;
using Microsoft.Extensions.DependencyInjection;
namespace Master.Infrastructure.Processing
{
    public static class QueriesExecutor
    {
        public static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using var scope = ServiceProviderLocator.CreateScope();
            var mediator = scope?.ServiceProvider.GetService<IMediator>();

            return await mediator?.Send(query)!;
        }
    }
}