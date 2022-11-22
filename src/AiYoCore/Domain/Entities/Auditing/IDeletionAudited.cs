namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 删除审计属性接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IDeletionAudited<TPrimaryKey> : IHasDeletionTime where TPrimaryKey : struct
    {
        /// <summary>
        /// 删除此实体的用户
        /// </summary>
        TPrimaryKey? DeleterUserId { get; set; }
    }
    /// <summary>
    /// 删除审计属性接口-主键为String类型
    /// </summary>
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// 最后删除人
        /// </summary>
        string DeleterUserId { get; set; }
    }
}
