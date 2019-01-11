using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using Dapper;
namespace Virgo.Dapperd
{
    public class DapperRepository : IDapperRepository
    {
        private readonly MySqlConnection _conn = new MySqlConnection("server=core-center.mysql.rds.aliyuncs.com;user id=cloud_app_admin02;password= Justdoit$ ;port=3306;database=builder_puma;sslmode=none;Connection Timeout=100;");
        public async Task Insert<TEntity>(TEntity entity)
        {
        Dapper.
        }
    }
}
