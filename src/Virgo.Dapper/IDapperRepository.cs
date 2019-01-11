using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Dapper
{
    public interface IDapperRepository
    {
        Task Insert<TEntity>(TEntity entity);
    }
}
