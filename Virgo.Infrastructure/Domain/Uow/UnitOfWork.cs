using MySql.Data.MySqlClient;
using System.Data;
using Virgo.Domain.Uow;

namespace Virgo.Infrastructure.Domain.Uow
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : UnitOfWorkBase
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection();
        }
    }
}
