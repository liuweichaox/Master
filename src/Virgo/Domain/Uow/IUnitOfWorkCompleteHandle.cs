using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 定义工作单元提交操作
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// 完成工作单元，
        /// 保存所有更改并提交事务
        /// </summary>
        void Complete();
    }
}
