namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 修改审计属性接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IModificationAudited<TPrimaryKey> : IHasModificationTime where TPrimaryKey : struct
    {
        /// <summary>
        /// 此实体的上次修改用户
        /// </summary>
        TPrimaryKey? LastModifierUserId { get; set; }
    }
    /// <summary>
    /// 修改审计属性接口-主键为String类型
    /// </summary>
    public interface IModificationAudited : IHasModificationTime
    {
        string LastModifierUserId { get; set; }
    }
}
