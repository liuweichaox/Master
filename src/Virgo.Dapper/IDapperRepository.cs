using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Virgo.Dapper
{
    /// <summary>
    /// Dapper仓储接口
    /// </summary>
    public interface IDapperRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        bool Delete(TEntity entityToDelete);
        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entities">要删除的实体</param>
        /// <returns></returns>
        bool Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity entityToDelete);
        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="entities">要删除的实体</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除所有实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <returns></returns>
        bool DeleteAll();

        /// <summary>
        /// 删除所有实体
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <returns></retrns>
        Task<bool> DeleuteAllAsync();

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        TEntity Get(dynamic id);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(dynamic id);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        long Insert(TEntity entityToInsert);
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entities">要插入的实体</param>
        /// <returns></returns>
        long Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity entityToInsert, ISqlAdapter sqlAdapter = null);
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">要插入的类型</typeparam>
        /// <param name="entities">要插入的实体</param>
        /// <returns></returns>
        Task<int> InsertAsync(IEnumerable<TEntity> entities, ISqlAdapter sqlAdapter = null);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entityToUpdate">要跟新的实体</param>
        /// <returns></returns>
        bool Update(TEntity entityToUpdate);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entities">要跟新的实体</param>
        /// <returns></returns>
        bool Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entityToUpdate">要跟新的实体</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entityToUpdate);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">要更新的类型</typeparam>
        /// <param name="entities">要跟新的实体</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 查询首行受列
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        TReturn ExecuteScalar<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 查询首行受列
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        IEnumerable<TReturn> Query<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 异步执行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询并映射第一个结果
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        TReturn QueryFirst<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询并映射第一个结果
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<TReturn> QueryFirstAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        TReturn QueryFirstOrDefault<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

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
        TReturn QuerySingle<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 使用.NET 4.5任务异步执行单行查询
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<TReturn> QuerySingleAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        TReturn QuerySingleOrDefault<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;

        /// <summary>
        /// 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="sql">要执行的查询</param>
        /// <param name="param">要传递的参数（如果有)</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        Task<TReturn> QuerySingleOrDefaultAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class;
    }
}
