using Quartz;
using System.Threading.Tasks;

namespace Virgo.Quartz
{
    public abstract class JobBase : IJob
    {
        public abstract Task Execute(IJobExecutionContext context);
    }
}
