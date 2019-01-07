using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Virgo.Quartz
{
    public class QuartzScheduleJobManager : IQuartzScheduleJobManager
    {
        private readonly IScheduler _scheduler;
        public QuartzScheduleJobManager()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        }
        public bool IsRunning { get { return _isRunning; } }

        private volatile bool _isRunning;

        public async Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob
        {
            var jobToBuild = JobBuilder.Create<TJob>();
            configureJob(jobToBuild);
            var job = jobToBuild.Build();

            var triggerToBuild = TriggerBuilder.Create();
            configureTrigger(triggerToBuild);
            var trigger = triggerToBuild.Build();
            await _scheduler.ScheduleJob(job, trigger);
        }

        public void Start()
        {
            _isRunning = true;
            _scheduler.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _scheduler.Shutdown(true);
        }
    }
}
