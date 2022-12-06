using MediatR;
using Velen.Infrastructure.Commands;
using ICommand = System.Windows.Input.ICommand;
using Microsoft.Extensions.DependencyInjection;

namespace Velen.Infrastructure.Processing
{
    public static class CommandsExecutor
    {
        public static async Task Execute(ICommand command)
        {
            using var scope = ServiceProviderLocator.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator?.Send(command)!;
        }

        public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using var scope = ServiceProviderLocator.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator?.Send(command)!;
        }
        
        public static async Task ExecuteAsync(ICommand command)
        {
            await using var scope = ServiceProviderLocator.CreateAsyncScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator?.Send(command)!;
        }

        public static async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            await using var scope = ServiceProviderLocator.CreateAsyncScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator?.Send(command)!;
        }
    }
}