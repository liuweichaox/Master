using System;
using System.Transactions;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 定义一个工作单元，
    /// 这个接口是框架内部使用。
    /// 应用程序使用<see cref="Begin(UnitOfWorkOptions)"/>来开启一个新的事务
    /// </summary>
    public interface IUnitOfWork : IActiveUnitOfWork, IDisposable
    {
        /// <summary>
        /// 环境事务
        /// </summary>
        TransactionScope Transaction { get; set; }
        /// <summary>
        /// 默认工作单元
        /// </summary>
        IUnitOfWork Begin();
        /// <summary>
        /// 根据给定选项开启工作单元
        /// </summary>
        /// <param name="options">工作单元选项<see cref="UnitOfWorkOptions"/></param>
        IUnitOfWork Begin(UnitOfWorkOptions options);
        /// <summary>
        /// 完成工作单元，保存所有更改并提交事务
        /// </summary>
        void Complete();
    }
}
