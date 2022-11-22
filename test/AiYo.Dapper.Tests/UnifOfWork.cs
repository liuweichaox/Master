using MySql.Data.MySqlClient;
using System.Data;
using Virgo.Domain.Uow;

namespace Virgo.Dapper.Tests
{
    public class UnitOfWork : UnitOfWorkBase
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection("server=localhost;port=3306;user=root;password=123456; database=AiYo;");
        }
    }
}
