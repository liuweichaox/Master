namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 创建/更新/删除审计相关属性接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IFullAudited<TPrimaryKey> : IAudited<TPrimaryKey>, IDeletionAudited<TPrimaryKey> where TPrimaryKey : struct
    {

    }
    /// <summary>
    /// 创建/更新/删除审计相关属性接口
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {

    }
}
