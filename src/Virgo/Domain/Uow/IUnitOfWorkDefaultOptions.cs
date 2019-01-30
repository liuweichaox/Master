using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 设置或者获取工作单元默认值
    /// </summary>
    public interface IUnitOfWorkDefaultOptions
    {
        /// <summary>
        /// 事务作用域选项
        /// </summary>
        TransactionScopeOption Scope { get; set; }
        /// <summary>
        /// 是否工作单元事务
        /// Default:true
        /// </summary>
        bool IsTransactional { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        TimeSpan? Timeout { get; set; }
        /// <summary>
        /// 事务隔离级别
        /// </summary>
        IsolationLevel? IsolationLevel { get; set; }
    }
}
