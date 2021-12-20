using Nito.AsyncEx;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Virgo.Quartz
{
    public class QuartzScheduleJobManager : IQuartzScheduleJobManager
    {
        private static volatile IScheduler _scheduler = null;
        public bool IsRunning => _isRunning;

        private volatile bool _isRunning;

        private static readonly AsyncLock AsyncLock = new AsyncLock();

        private async Task<IScheduler> SchedulerAsync()
        {
            if (_scheduler != null) return _scheduler;
            using (await AsyncLock.LockAsync())
            {
                if (_scheduler != null) return _scheduler;
                var factory = new StdSchedulerFactory();
                _scheduler = await factory.GetScheduler();
            }
            return _scheduler;
        }

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

        public async Task StartAsync()
        {
            _isRunning = true;
            _scheduler = await SchedulerAsync();
            await _scheduler.Start();
        }

        public async Task StopAsync()
        {
            _isRunning = false;
            await _scheduler.Shutdown(true);
        }
    }
}
