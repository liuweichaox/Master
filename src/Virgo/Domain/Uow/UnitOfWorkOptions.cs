using System;
using System.Transactions;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 工作单元选设置
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 事务作用域
        /// </summary>
        public TransactionScopeOption? Scope { get; set; } = TransactionScopeOption.Required;
        /// <summary>
        /// 是否事务性
        /// </summary>
        public bool? IsTransactional { get; set; } = true;
        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
    }
}
