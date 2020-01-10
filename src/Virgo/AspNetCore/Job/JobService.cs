using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Virgo.AspNetCore.Job
{
    /// <summary>
    /// 重写<see cref="IHostedService"/>
    /// </summary>
    public abstract class VirgoBackgroundService : IHostedService
    {
        private Task _executingTask;
        /// <summary>
        /// 取消令牌
        /// </summary>
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        /// <summary>
        /// 定时Timer实例
        /// </summary>
        private Timer _timer;
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (Interval.HasValue)
            {
                _timer = new Timer(DoWork, null, TimeSpan.Zero, Interval.Value);
            }
            else
            {
                DoWork(null);
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 需要执行的方法
        /// </summary>
        /// <param name="state"></param>
        public abstract void DoWork(object state);

        /// <summary>
        /// 间隔时间，空则值执行一次
        /// </summary>
        public TimeSpan? Interval { get; set; }
        /// <summary>
        /// 开始时执行的操作
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {

            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        /// <summary>
        /// 结束时执行的操作
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
