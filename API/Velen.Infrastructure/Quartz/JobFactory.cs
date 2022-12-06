using Quartz;
using Quartz.Spi;

namespace Velen.Infrastructure.Quartz
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _provider;

        public JobFactory(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _provider.GetService(bundle.JobDetail.JobType);
                
            return job  as IJob;
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}