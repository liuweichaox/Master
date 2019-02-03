using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Dapper
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交事务操作
        /// </summary>
        /// <param name="action">改变数据的操作</param>
        void Commit(Action action);
    }
}
