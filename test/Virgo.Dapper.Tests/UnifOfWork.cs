using MySql.Data.MySqlClient;
using System.Data;
using Virgo.Domain.Uow;

namespace Virgo.Dapper.Tests
{
    public class UnifOfWork : UnitOfWorkBase
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection("");
        }
    }
}
