using System;
using System.Threading.Tasks;
using Virgo.Domain.Uow;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 定义当前<see cref="IUnitOfWork"/> 的基本操作
    /// </summary>
    public interface IActiveUnitOfWork
    {
        /// <summary>
        /// 事务提交成功事件
        /// </summary>
        event EventHandler Completed;
        /// <summary>
        /// 事务提交失败事件
        /// </summary>
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        /// <summary>
        /// 事务释放时事件
        /// </summary>
        event EventHandler Disposed;
        /// <summary>
        /// 当前资源是否已释放
        /// </summary>
        bool IsDisposed { get; }        
    }
}
