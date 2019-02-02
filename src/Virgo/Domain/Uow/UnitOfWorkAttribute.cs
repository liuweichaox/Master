using System;
using System.Transactions;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 标记方法是否使用事务提交，如果标记启用事务提交，则所有操作将在打开数据库后一并提交，失败将回滚
    /// </summary>
    /// <remarks>
    /// 如果调用此方法之外已存在一个工作单元，并不会影响，因为他们将会使用同一个事务提交
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// 事务作用域
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 设置是否禁用工作单元
        /// </summary>
        public bool IsDisabled { get; set; }
        public UnitOfWorkAttribute()
        {

        }
        /// <summary>
        /// 创建<see cref="UnitOfWorkAttribute"/>新实例
        /// </summary>
        /// <param name="isDisabled">设置是否禁用工作单元,默认false</param>
        public UnitOfWorkAttribute(bool isDisabled)
        {
            IsDisabled = isDisabled;
        }
        /// <summary>
        /// 创建<see cref="UnitOfWorkAttribute"/>新实例
        /// </summary>
        /// <param name="scope">事务作用域</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope)
        {
            Scope = scope;
        }
        /// <summary>
        /// 创建<see cref="UnitOfWorkAttribute"/>新实例
        /// </summary>
        /// <param name="scope">事务作用域</param>
        /// <param name="timeOut">超时时间，单位（秒）</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope, int timeOut)
        {
            Scope = scope;
            Timeout = TimeSpan.FromSeconds(timeOut);
        }
        /// <summary>
        /// 创建<see cref="UnitOfWorkAttribute"/>新实例
        /// </summary>
        /// <param name="scope">事务作用域</param>
        /// <param name="isolationLevel">事务隔离级别</param>
        /// <param name="timeOut">超时时间，单位（秒）</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope, IsolationLevel isolationLevel,int timeOut)
        {
            Scope = scope;
            Timeout = TimeSpan.FromSeconds(timeOut);
        }
        internal UnitOfWorkOptions CreateOptions()
        {
            return new UnitOfWorkOptions
            {
                IsolationLevel = IsolationLevel,
                Timeout = Timeout,
                Scope = Scope
            };
        }
    }
}
