using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Quartz
{
    public interface IQuartzConfiguration
    {
        IScheduler Scheduler { get; }
    }
}
