using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Virgo.Dapper
{
    /// <summary>
    /// 上下文接口
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// 指示事务是否已启动
        /// </summary>
        bool IsTransactionStarted { get; }

        /// <summary>
        /// 开始事务处理
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务操作
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务的操作
        /// </summary>
        void Rollback();
    }
}
