using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Dapper
{
    /// <summary>
    /// <see cref="IUnitOfWork"/>的实现类
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Dapper上下文
        /// </summary>
        private readonly IContext _context;

        /// <summary>
        /// 创建Dapper上下文实例，开启新的数据库事务
        /// </summary>
        /// <param name="context">Dapper上下文</param>
        public UnitOfWork(IContext context)
        {
            _context = context;

            // 开始事务
            _context.BeginTransaction();
        }

        /// <summary>
        /// 将更改保存到上下文中
        /// </summary>
        public void SaveChanges()
        {

        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (!_context.IsTransactionStarted)
                throw new InvalidOperationException("Transaction have already been commited or disposed.");

            // 提交事务
            _context.Commit();
        }

        public void Rollback()
        {
            if (_context.IsTransactionStarted)
            {
                // 回滚事务
                _context.Rollback();
            }
        }
    }
}
