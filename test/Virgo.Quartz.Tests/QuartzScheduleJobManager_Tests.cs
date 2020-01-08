using Autofac;
using Quartz;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Virgo.TestBase;
using Xunit;

namespace Virgo.Quartz.Tests
{
    public class QuartzScheduleJobManager_Tests : TestBaseWithIocBuilder
    {
        private readonly IQuartzScheduleJobManager _scheduleJobManager;
        public QuartzScheduleJobManager_Tests()
        {
            Building(builder =>
            {
                builder.RegisterType<QuartzScheduleJobManager>().As<IQuartzScheduleJobManager>().InstancePerDependency();
            });
            _scheduleJobManager = The<IQuartzScheduleJobManager>();
        }
        [Fact]
        public async Task Simple_Job_Test()
        {
            await _scheduleJobManager.StartAsync();
            await ScheduleJobsAsync();
            Thread.Sleep(100);
            TestJob1.TestNum1.ShouldBeGreaterThan(1);
            TestJob2.TestNum2.ShouldBeGreaterThan(1);
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
                     .WithSimpleSchedule(schedule => schedule.WithRepeatCount(100).WithInterval(TimeSpan.FromMilliseconds(1)))
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
                     .WithSimpleSchedule(schedule => schedule.WithRepeatCount(100).WithInterval(TimeSpan.FromMilliseconds(1)))
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
