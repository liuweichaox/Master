using Autofac.Extras.IocManager;
using Quartz;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Virgo.Quartz.Tests
{
    public class QuartzScheduleJobManager_Tests
    {
        private readonly IQuartzScheduleJobManager _scheduleJobManager;
        public QuartzScheduleJobManager_Tests()
        {
            IRootResolver resolver = IocBuilder.New.UseAutofacContainerBuilder()
                                      .RegisterServices(r =>
                                      {
                                          r.Register(typeof(IQuartzConfiguration), typeof(QuartzConfiguration), Lifetime.Singleton);
                                          r.Register(typeof(IQuartzScheduleJobManager), typeof(QuartzScheduleJobManager), Lifetime.Transient);
                                      })
                                      .RegisterIocManager()
                                      .CreateResolver()
                                      .UseIocManager();
            _scheduleJobManager = resolver.Resolve<IQuartzScheduleJobManager>();
        }
        [Fact]
        public async Task Simple_Job_Test()
        {
            _scheduleJobManager.Start();
            await ScheduleJobsAsync();
            Thread.Sleep(1000);
            TestJob1.TestNum1.ShouldBeGreaterThan(100);
            TestJob2.TestNum2.ShouldBeGreaterThan(100);
        }
        private async Task ScheduleJobsAsync()
        {
            await _scheduleJobManager.ScheduleAsync<TestJob1>(
                 job =>
                 {
                     job.WithIdentity("Test1JobKey").WithDescription("Test1 Job Description").Build();
                 },
                 trigger =>
                 {
                     trigger.WithIdentity("TestJob1Trigger")
                     .WithDescription("TestJob1Trigger Description")
                     .WithSimpleSchedule(schedule => schedule.WithRepeatCount(1000).WithInterval(TimeSpan.FromMilliseconds(1)))
                     .StartNow();
                 });

            await _scheduleJobManager.ScheduleAsync<TestJob2>(
                 job =>
                 {
                     job.WithIdentity("TestJob2Trigger").WithDescription("Test2 Job Description").Build();
                 },
                 trigger =>
                 {
                     trigger.WithIdentity("HelloJob2Trigger")
                     .WithDescription("TestJob1Trigger Description")
                     .WithSimpleSchedule(schedule => schedule.WithRepeatCount(1000).WithInterval(TimeSpan.FromMilliseconds(1)))
                     .StartNow();
                 });

        }

        public class TestJob1 : JobBase
        {
            public static int TestNum1 = -1;
            public override Task Execute(IJobExecutionContext context)
            {
                TestNum1++;
                return Task.CompletedTask;
            }
        }
        public class TestJob2 : JobBase
        {
            public static int TestNum2 = -1;
            public override Task Execute(IJobExecutionContext context)
            {
                TestNum2++;
                return Task.CompletedTask;
            }
        }
    }
}
