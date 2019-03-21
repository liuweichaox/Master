using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Virgo.Dapper
{
    public abstract class UnitOfWorkBase : IUnitOfWork, ILifetimeScopeDependency
    {
        public UnitOfWorkBase(DbConfiguration configuration)
        {
            CommandTimeout = configuration?.CommandTimeout;
            Connection = CreateConnection();
        }
        public IDbTransaction Transaction { get; set; }
        public IDbConnection Connection { get; }
        public int? CommandTimeout { get; }
        public bool IsTransactionStarted { get; set; }
        public abstract IDbConnection CreateConnection();
        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
            IsTransactionStarted = true;
        }
        public void Commit()
        {
            if (!IsTransactionStarted)
            {
                throw new InvalidOperationException("Transaction have already been commited or disposed.");
            }
            Transaction.Commit();
            IsTransactionStarted = false;
        }
        public void Rollback()
        {
            if (IsTransactionStarted)
            {
                Transaction.Rollback();
            }
        }
    }
}
