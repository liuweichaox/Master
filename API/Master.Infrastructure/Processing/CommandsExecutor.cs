using System.Text.Json;
using MediatR;
using Master.Infrastructure.Commands;
using ICommand = System.Windows.Input.ICommand;
using Microsoft.Extensions.DependencyInjection;

namespace Master.Infrastructure.Processing
{
    public static class CommandsExecutor
    {
        public static async Task Execute(ICommand command)
        {
            Console.WriteLine("CommandsExecutor Execute");
            using var scope = ServiceProviderLocator.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            await mediator?.Send(command)!;
        }

        public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            Console.WriteLine("CommandsExecutor Execute TResult,command json "+JsonSerializer.Serialize(command)+command.GetType().Name);
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