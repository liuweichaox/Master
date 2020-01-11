using System;

namespace Virgo.Domain.Entities
{

    /// <summary>
    /// <see cref="IEntity"/>抽象实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 自增主键[Key]
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }
    }
    /// <summary>
    /// <see cref="IEntity"/>抽象实现类-主键为String类型
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// 非自增主键[ExplicitKey]
        /// </summary>
        public virtual string Id { get; set; }
    }
}
