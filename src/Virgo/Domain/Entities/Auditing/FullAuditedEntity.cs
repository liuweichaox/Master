using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// <see cref="IFullAudited"/> 完整审计基类抽象实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体主键类型</typeparam>
    [Serializable]
    public class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 删除用户
        /// </summary>
        public virtual TPrimaryKey? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
    /// <summary>
    /// <see cref="IFullAudited"/> 完整审计基类抽象实现类-主键为String类型
    /// </summary>
    [Serializable]
    public class FullAuditedEntity : AuditedEntity, IFullAudited
    {
        public virtual bool IsDeleted { get; set; }

        public virtual string DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
    }
}
