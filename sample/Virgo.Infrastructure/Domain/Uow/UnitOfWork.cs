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
            return new MySqlConnection("server=core.lyrewing.com;user id=root;password=Justdoit2018$;database=aio;SslMode=none");
        }
    }
}
