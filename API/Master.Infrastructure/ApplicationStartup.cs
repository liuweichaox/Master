using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Master.Infrastructure.Processing.InternalCommands;
using Master.Infrastructure.Processing.Outbox;

namespace Master.Infrastructure;

public class ApplicationStartup
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();
        
        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
        var trigger =
            TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule("0/15 * * ? * *")
                .Build();

        await scheduler.ScheduleJob(processOutboxJob, trigger);

        var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
        var triggerCommandsProcessing =
            TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule("0/15 * * ? * *")
                .Build();
        await scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing);
    }
}