using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Quartz
{
    /// <summary>
    /// 用于启动/停止自线程服务的接口
    /// </summary>
    public interface IQuartzScheduleJobManager
    {
        /// <summary>
        /// 用于控制运行的布尔值
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        /// 启动服务
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// 向服务发送stop命令
        /// 服务可能会立即返回并异步停止
        /// 然后客户端应该调用<see cref =“WaitToStop”/>方法以确保它已停止
        /// </summary>
        Task StopAsync();
        /// <summary>
        /// 安排要执行的作业
        /// </summary>
        /// <typeparam name="TJob">工作类型</typeparam>
        /// <param name="configureJob">要构建的作业特定定义</param>
        /// <param name="configureTrigger">作业特定的触发选项，时间间隔</param>
        /// <returns></returns>
        Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob;
    }
}
