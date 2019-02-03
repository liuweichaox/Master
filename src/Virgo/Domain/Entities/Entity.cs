using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities
{
    /// <summary>
    /// <see cref="IEntity"/>抽象实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        public virtual TPrimaryKey Id { get; set; }
    }
    /// <summary>
    /// <see cref="IEntity"/>抽象实现类-主键为String类型
    /// </summary>
    public abstract class Entity : IEntity
    {
        public virtual string Id { get; set; }
    }
}
