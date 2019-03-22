using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Uow
{
    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork Create()
        {
            return new UnifOfWork();
        }
    }
}
