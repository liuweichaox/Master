using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Virgo.Domain.Uow;

namespace Virgo.Dapper.Tests
{
    public class UnifOfWork : UnitOfWorkBase
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection("server=core.lyrewing.com;user id=root;password=Justdoit2018$;database=Virgo;SslMode=none");
        }
    }
}
