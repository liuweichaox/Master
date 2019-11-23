using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 抽象实现<see cref ="IAudited"/>的聚合根
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey> : CreationAuditedAggregateRoot<TPrimaryKey>, IAudited<TPrimaryKey> where TPrimaryKey : struct
    {
        /// <summary>
        /// 此实体的上次修改日期
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 上次修改此实体的用户
        /// </summary>
        public virtual TPrimaryKey? LastModifierUserId { get; set; }
    }

    /// <summary>
    /// 抽象实现<see cref ="IAudited"/>的聚合根-主键为String类型
    /// </summary>
    [Serializable]
    public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IAudited
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual string LastModifierUserId { get; set; }
    }
}
