using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Velen.Infrastructure.Processing.InternalCommands;
using Velen.Infrastructure.Processing.Outbox;

namespace Velen.Infrastructure;

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