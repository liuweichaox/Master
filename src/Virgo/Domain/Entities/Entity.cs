using Dapper.Contrib.Extensions;
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
        //Dapper区别：
        //      ExplicitKey 不自增主键
        //      Key 自增主键
        [ExplicitKey]
        public virtual TPrimaryKey Id { get; set; }
    }
    /// <summary>
    /// <see cref="IEntity"/>抽象实现类-主键为String类型
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {
        [ExplicitKey]
        public virtual string Id { get; set; }
    }
}
