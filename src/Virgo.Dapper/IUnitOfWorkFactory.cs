using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Dapper
{
    /// <summary>
    /// 创建工作单元的工厂
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// 创建工作单元的实例
        /// </summary>
        /// <returns>工作单元</returns>
        IUnitOfWork Create();
    }
}
