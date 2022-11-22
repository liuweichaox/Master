namespace Virgo.Domain.Entities
{
    /// <summary>
    /// 定义基本实体类型的接口，系统中的所有实体都必须实现此接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体的主键类型</typeparam>
    public interface IEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 此实体的唯一标识符
        /// </summary>        
        TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// 定义基本实体类型的接口，系统中的所有实体都必须实现此接口-主键为String类型
    /// </summary>
    public interface IEntity
    {
        string Id { get; set; }
    }
}
