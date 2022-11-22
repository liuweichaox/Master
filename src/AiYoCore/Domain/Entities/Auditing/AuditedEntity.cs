using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// <see cref="IAudited"/>实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAudited<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改用户
        /// </summary>
        public virtual TPrimaryKey? LastModifierUserId { get; set; }
    }
    /// <summary>
    ///  <see cref="IAudited"/>实现类-主键为String类型
    /// </summary>
    [Serializable]
    public abstract class AuditedEntity : CreationAuditedEntity, IAudited
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public virtual string LastModifierUserId { get; set; }
    }
}
