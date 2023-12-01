using Quartz;

namespace Master.Infrastructure.Processing.InternalCommands
{
    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("ProcessInternalCommandsJob  IJob");
            await CommandsExecutor.Execute(new ProcessInternalCommandsCommand());
        }
    }
}