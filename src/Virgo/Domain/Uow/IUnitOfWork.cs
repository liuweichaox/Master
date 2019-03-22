using System;
using System.Data;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IActiveUnitOfWork, IDisposable
    {
        /// <summary>
        /// 事务
        /// </summary>
        IDbTransaction Transaction { get; set; }
        /// <summary>
        /// 连接器
        /// </summary>
        IDbConnection Connection { get; }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        void BeginTransaction(IsolationLevel? isolationLevel = null);
        /// <summary>
        /// 提交事务操作
        /// </summary>
        void Commit();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void Rollback();
    }
}
