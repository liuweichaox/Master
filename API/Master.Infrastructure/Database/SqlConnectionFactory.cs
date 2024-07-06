using System.Data;
using Master.Domain.Data;
using MySql.Data.MySqlClient;

namespace Master.Infrastructure.Database;

public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly string? _connectionString;
    private IDbConnection _connection;

    public SqlConnectionFactory(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open) _connection.Dispose();
    }

    public IDbConnection GetOpenConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        return _connection;
    }
}