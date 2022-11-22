namespace Virgo.Domain.Entities
{
    /// <summary>
    /// 聚合根实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体主键类型</typeparam>
    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey> where TPrimaryKey : struct
    {
    }
    /// <summary>
    /// 聚合跟实现类-主键为String类型
    /// </summary>
    public class AggregateRoot : Entity, IAggregateRoot
    {
    }
}
