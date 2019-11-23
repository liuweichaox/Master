using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// <see cref ="IFullAudited"/>完整审计的聚合根抽象实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    [Serializable]
    public class FullAuditedAggregateRoot<TPrimaryKey> : AuditedAggregateRoot<TPrimaryKey>, IFullAudited<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 此实体是否已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 哪个用户删除了这个实体
        /// </summary>
        public virtual TPrimaryKey? DeleterUserId { get; set; }

        /// <summary>
        /// 删除此实体的时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
    /// <summary>
    /// <see cref ="IFullAudited"/>完整审计的聚合根抽象实现类-主键为String类型
    /// </summary>
    [Serializable]
    public class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAudited
    {
        public virtual bool IsDeleted { get; set; }

        public virtual string DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
