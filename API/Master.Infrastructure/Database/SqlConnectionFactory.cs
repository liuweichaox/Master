using System.Data;
using MySql.Data.MySqlClient;
using Master.Domain.Data;

namespace Master.Infrastructure.Database;

public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly string? _connectionString;
    private IDbConnection _connection;

    public SqlConnectionFactory(string? connectionString)
    {
        this._connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
        if (this._connection == null || this._connection.State != ConnectionState.Open)
        {
            this._connection = new MySqlConnection(_connectionString);
            this._connection.Open();
        }

        return this._connection;
    }

    public void Dispose()
    {
        if (this._connection != null && this._connection.State == ConnectionState.Open)
        {
            this._connection.Dispose();
        }
    }
}