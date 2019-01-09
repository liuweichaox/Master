using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities
{
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
