using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Virgo.Domain.Uow;

namespace Virgo.Infrastructure.Sample.Domain.Uow
{
    public class UnitOfWork : UnitOfWorkBase
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection();
        }
    }
}
