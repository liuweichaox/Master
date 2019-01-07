using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Virgo.Quartz
{
    public class QuartzScheduleJobManager : IQuartzScheduleJobManager
    {
        private readonly IQuartzConfiguration _quartzConfiguration;
        public QuartzScheduleJobManager(IQuartzConfiguration quartzConfiguration)
        {
            _quartzConfiguration = quartzConfiguration;
        }
        public bool IsRunning { get { return _isRunning; } }

        private volatile bool _isRunning;

        public async Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger)where TJob : IJob
        {
            var jobToBuild = JobBuilder.Create<TJob>();
            configureJob(jobToBuild);
            var job = jobToBuild.Build();

            var triggerToBuild = TriggerBuilder.Create();
            configureTrigger(triggerToBuild);
            var trigger = triggerToBuild.Build();
            await _quartzConfiguration.Scheduler.ScheduleJob(job, trigger);
        }

        public void Start()
        {
            _isRunning = true;
            _quartzConfiguration.Scheduler.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _quartzConfiguration.Scheduler.Shutdown(true);
        }
    }
}
