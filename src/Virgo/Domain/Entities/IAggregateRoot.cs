namespace Virgo.Domain.Entities
{
    /// <summary>
    /// 聚合根接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体主键类型</typeparam>
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey> where TPrimaryKey : struct
    {

    }
    /// <summary>
    /// 聚合根接口-主键为String类型
    /// </summary>
    public interface IAggregateRoot : IEntity
    {

    }
}
