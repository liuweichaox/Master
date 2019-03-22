using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Virgo.Domain.Uow
{
    public class UnifOfWork : UnitOfWorkBase
    {
        public override IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}
