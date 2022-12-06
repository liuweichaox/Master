using Velen.Infrastructure.Commands;

namespace Velen.Infrastructure.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}