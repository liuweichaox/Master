using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Dapper.Contrib.Extensions;
using Dapper;
using static Dapper.SqlMapper;

namespace Virgo.Dapper
{
    /// <summary>
    /// Daaper上下文基类
    /// </summary>
    public abstract class ContextBase : IContext
    {
        #region 字段

        /// <summary>
        /// 事务是否已启动
        /// </summary>
        private bool _isTransactionStarted;

        /// <summary>
        /// 数据库的连接对象
        /// </summary>
        private IDbConnection _connection;

        /// <summary>
        /// 要连接的事务对象
        /// </summary>
        private IDbTransaction _transaction;

        /// <summary>
        /// 命令超时
        /// </summary>
        private int? _commandTimeout = null;

        #endregion Fields

        #region 属性

        /// <summary>
        /// 事务是否已启动
        /// </summary>
        public bool IsTransactionStarted
        {
            get
            {
                return _isTransactionStarted;
            }
        }

        #endregion Properties

        #region 抽象方法

        /// <summary>
        /// 创建到数据库的连接对象
        /// </summary>
        /// <returns>连接对象</returns>
        protected abstract IDbConnection CreateConnection();

        #endregion Abstract methods

        #region 方法

        protected ContextBase()
        {
            _connection = CreateConnection();
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            DebugPrint("Connection started.");
        }

        private void DebugPrint(string message)
        {
#if DEBUG
            Debug.Print(">>> UnitOfWork With Dapper - Thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, message);
#endif
        }

        #region Transaction

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (_isTransactionStarted)
                throw new InvalidOperationException("Transaction is already started.");

            _transaction = _connection.BeginTransaction();

            _isTransactionStarted = true;

            DebugPrint("Transaction started.");
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (!_isTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Commit();

            _transaction = null;

            _isTransactionStarted = false;

            DebugPrint("Transaction committed.");
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (!_isTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            _isTransactionStarted = false;

            DebugPrint("Transaction rollbacked and disposed.");
        }

        #endregion Transaction

        #region Execute
        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        public bool Delete<T>(T entityToDelete) where T : class
        {
            return SqlMapperExtensions.Delete<T>(_connection, entityToDelete, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(T entityToDelete) where T : class
        {
            return await SqlMapperExtensions.DeleteAsync<T>(_connection, entityToDelete, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 删除所有实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <returns></returns>
        public bool DeleteAll<T>() where T : class
        {
            return SqlMapperExtensions.DeleteAll<T>(_connection, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 删除所有实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <returns></returns>
        public async Task<bool> DeleteAllAsync<T>() where T : class
        {
            return await SqlMapperExtensions.DeleteAllAsync<T>(_connection, _transaction, _commandTimeout);
        }

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            return SqlMapperExtensions.GetAll<T>(_connection, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await SqlMapperExtensions.GetAllAsync<T>(_connection, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T Get<T>(dynamic id) where T : class
        {
            return SqlMapperExtensions.Get<T>(_connection, id, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(dynamic id) where T : class
        {
            return await SqlMapperExtensions.GetAsync<T>(_connection, id, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        public long Insert<T>(T entityToInsert) where T : class
        {
            return SqlMapperExtensions.Insert<T>(_connection, entityToInsert, _transaction, _commandTimeout);
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        public async Task<int> InsertAsync<T>(T entityToInsert, ISqlAdapter sqlAdapter = null) where T : class
        {
            return await SqlMapperExtensions.InsertAsync<T>(_connection, entityToInsert, _transaction, _commandTimeout, sqlAdapter);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entityToUpdate">要跟新的实体</param>
        /// <returns></returns>
        public bool Update<T>(T entityToUpdate) where T : class
        {
            return SqlMapperExtensions.Update<T>(_connection, entityToUpdate, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entityToUpdate">要跟新的实体</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T>(T entityToUpdate) where T : class
        {
            return await SqlMapperExtensions.UpdateAsync<T>(_connection, entityToUpdate, _transaction, _commandTimeout);
        }
        /// <summary>
        /// 查询首行受列
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        public T ExecuteScalar<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.ExecuteScalar<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 查询首行受列
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.ExecuteScalarAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.Query<T>(_connection, sql, param, _transaction, false, _commandTimeout, commandType);
        }
        /// <summary>
        /// 异步执行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QueryAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行查询并映射第一个结果
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryFirst<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行查询并映射第一个结果
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QueryFirstAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryFirstOrDefault<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QueryFirstOrDefaultAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行返回多个结果集的命令，然后依次访问每个结果集
        /// </summary>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public GridReader QueryMultiple(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行返回多个结果集的命令，然后依次访问每个结果集
        /// </summary>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QueryMultipleAsync(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 执行单行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.QuerySingle<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }
        /// <summary>
        /// 使用.NET 4.5任务异步执行单行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QuerySingleAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        public T QuerySingleOrDefault<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.QuerySingleOrDefault<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QuerySingleOrDefaultAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        #endregion Query

        public void Dispose()
        {
            if (_isTransactionStarted)
                Rollback();

            _connection.Close();
            _connection.Dispose();
            _connection = null;

            DebugPrint("Connection closed and disposed.");
        }

        #endregion Methods
    }
}
