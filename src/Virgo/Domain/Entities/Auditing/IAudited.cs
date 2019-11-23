namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 创建/更新审计属性接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IAudited<TPrimaryKey> : ICreationAudited<TPrimaryKey>, IModificationAudited<TPrimaryKey> where TPrimaryKey : struct
    {

    }
    /// <summary>
    /// 创建/更新审计属性接口-主键为String类型
    /// </summary>
    public interface IAudited : ICreationAudited, IModificationAudited
    {

    }
}
