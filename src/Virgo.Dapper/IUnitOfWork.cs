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
        /// 将更改保存到上下文中
        /// </summary>
        void SaveChanges();
    }
}
