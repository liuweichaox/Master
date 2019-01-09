using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities
{
    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey> where TPrimaryKey : struct
    {
    }

    public class AggregateRoot : Entity, IAggregateRoot
    {
    }
}
