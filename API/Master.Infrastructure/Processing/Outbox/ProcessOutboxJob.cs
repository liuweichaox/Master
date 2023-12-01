using Quartz;

namespace Master.Infrastructure.Processing.Outbox
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Processing outbox IJob");
            await CommandsExecutor.Execute(new ProcessOutboxCommand());
        }
    }
}