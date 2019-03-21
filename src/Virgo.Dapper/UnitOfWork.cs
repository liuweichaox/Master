using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Virgo.Dapper
{
    public class UnitOfWork : UnitOfWorkBase
    {
        private readonly DbConfiguration _configuration;
        public UnitOfWork(DbConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.ConnectionString;
        }
    }
}
