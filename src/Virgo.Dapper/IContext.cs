using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Virgo.Dapper
{
    /// <summary>
    /// 上下文接口
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// 指示事务是否已启动
        /// </summary>
        bool IsTransactionStarted { get; }

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        bool Delete<T>(T entityToDelete) where T : class;

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        Task<bool> DeleteAsync<T>(T entityToDelete) where T : class;

        /// <summary>
        /// 删除所有实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <returns></returns>
        bool DeleteAll<T>() where T : class;

        /// <summary>
        /// 删除所有实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <returns></returns>
        Task<bool> DeleteAllAsync<T>() where T : class;

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>() where T : class;

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        T Get<T>(dynamic id) where T : class;

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(dynamic id) where T : class;

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        long Insert<T>(T entityToInsert) where T : class;

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(T entityToInsert, ISqlAdapter sqlAdapter = null) where T : class;

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entityToUpdate">要跟新的实体</param>
        /// <returns></returns>
        bool Update<T>(T entityToUpdate) where T : class;

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entityToUpdate">要跟新的实体</param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T entityToUpdate) where T : class;

        /// <summary>
        /// 查询首行受列
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        T ExecuteScalar<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 查询首行受列
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 异步执行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并映射第一个结果
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        T QueryFirst<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并映射第一个结果
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<T> QueryFirstAsync<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行返回多个结果集的命令，然后依次访问每个结果集
        /// </summary>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        GridReader QueryMultiple(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行返回多个结果集的命令，然后依次访问每个结果集
        /// </summary>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<GridReader> QueryMultipleAsync(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行单行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        T QuerySingle<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 使用.NET 4.5任务异步执行单行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        T QuerySingleOrDefault<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null);

        /// <summary>
        /// 开始事务处理
        /// </summary>
        void BeginTransaction();

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
