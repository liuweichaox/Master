using Master.Infrastructure.Commands;

namespace Master.Infrastructure.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}