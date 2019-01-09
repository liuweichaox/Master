using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities
{
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        public TPrimaryKey Id { get; set; }
    }
    public abstract class Entity : IEntity
    {
        public string Id { get; set; }
    }
}
