using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Virgo.Dapper
{
    /// <summary>
    /// <see cref="IUnitOfWork"/>的实现类
    /// </summary>
    public class UnitOfWork : IUnitOfWork,ISingletonDependency
    {
        public UnitOfWork()
        {
        }
        /// <summary>
        /// 提交事务操作
        /// </summary>
        /// <param name="action">改变数据的操作</param>
        public void Commit(Action action)
        {
            using (TransactionScope transaction=new TransactionScope())
            {
                action?.Invoke();
                transaction.Complete();
            }
        }
    }
}
