using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Dapper
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
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
