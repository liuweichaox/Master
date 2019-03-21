using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Virgo.Dapper
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
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
        /// 执行命令超时时间
        /// </summary>
        int? CommandTimeout { get; }
        /// <summary>
        /// 是否开启事务
        /// </summary>
        bool IsTransactionStarted { get; set; }
        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();
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
