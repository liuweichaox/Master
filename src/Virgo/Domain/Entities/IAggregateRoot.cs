using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities
{
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {

    }
}
