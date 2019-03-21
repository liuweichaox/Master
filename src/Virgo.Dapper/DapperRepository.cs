using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Virgo.Domain.Entities;
using static Dapper.SqlMapper;

namespace Virgo.Dapper
{
    /// <summary>
    /// Dapper仓储抽象实现类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
    {
        protected readonly IDbTransaction _transaction;
        protected readonly IDbConnection _connection;
        protected readonly int? _commandTimeout;
        public DapperRepository(IUnitOfWork unitOfWork)
        {
            _commandTimeout = unitOfWork.CommandTimeout;
            _connection = unitOfWork.Connection;
            _transaction = unitOfWork.Transaction;
        }

        public abstract IDbConnection CreateDbConnection();

        public bool Delete(TEntity entityToDelete)
        {
            return SqlMapperExtensions.Delete(_connection, entityToDelete, _transaction, _commandTimeout);
        }

        public bool DeleteAll()
        {
            return SqlMapperExtensions.DeleteAll<TEntity>(_connection, _transaction, _commandTimeout);
        }

        public async Task<bool> DeleteAllAsync()
        {
            return await SqlMapperExtensions.DeleteAllAsync<TEntity>(_connection, _transaction, _commandTimeout);
        }

        public async Task<bool> DeleteAsync(TEntity entityToDelete)
        {
            return await SqlMapperExtensions.DeleteAsync(_connection, entityToDelete, _transaction, _commandTimeout);
        }

        public TReturn ExecuteScalar<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return SqlMapper.ExecuteScalar<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return await SqlMapper.ExecuteScalarAsync<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public TEntity Get(dynamic id)
        {
            return SqlMapperExtensions.Get<TEntity>(_connection, id, _transaction, _commandTimeout);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return SqlMapperExtensions.GetAll<TEntity>(_connection, _transaction, _commandTimeout);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await SqlMapperExtensions.GetAllAsync<TEntity>(_connection, _transaction, _commandTimeout);
        }

        public async Task<TEntity> GetAsync(dynamic id)
        {
            return await SqlMapperExtensions.GetAsync<TEntity>(_connection, id, _transaction, _commandTimeout);
        }

        public long Insert(TEntity entityToInsert)
        {
            return SqlMapperExtensions.Insert(_connection, entityToInsert, _transaction, _commandTimeout);
        }

        public async Task<int> InsertAsync(TEntity entityToInsert, ISqlAdapter sqlAdapter = null)
        {
            return await SqlMapperExtensions.InsertAsync(_connection, entityToInsert, _transaction, _commandTimeout, sqlAdapter);
        }

        public IEnumerable<TReturn> Query<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return SqlMapper.Query<TReturn>(_connection, sql, param, _transaction, true, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return await SqlMapper.QueryAsync<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public TReturn QueryFirst<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return SqlMapper.QueryFirst<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<TReturn> QueryFirstAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return await SqlMapper.QueryFirstAsync<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public TReturn QueryFirstOrDefault<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return SqlMapper.QueryFirstOrDefault<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return await SqlMapper.QueryFirstOrDefaultAsync<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public GridReader QueryMultiple(string sql, object param = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, CommandType? commandType = null)
        {
            return await SqlMapper.QueryMultipleAsync(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public TReturn QuerySingle<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return SqlMapper.QuerySingle<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<TReturn> QuerySingleAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return await SqlMapper.QuerySingleAsync<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public TReturn QuerySingleOrDefault<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return SqlMapper.QuerySingleOrDefault<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<TReturn> QuerySingleOrDefaultAsync<TReturn>(string sql, object param = null, CommandType? commandType = null) where TReturn : class
        {
            return await SqlMapper.QuerySingleOrDefaultAsync<TReturn>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public bool Update(TEntity entityToUpdate)
        {
            return SqlMapperExtensions.Update(_connection, entityToUpdate, _transaction, _commandTimeout);
        }

        public async Task<bool> UpdateAsync(TEntity entityToUpdate)
        {
            return await SqlMapperExtensions.UpdateAsync(_connection, entityToUpdate, _transaction, _commandTimeout);
        }
    }
}
