using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 抽象实现<see cref ="IFullAudited"/>完整审计的聚合根
    /// </summary>
    /// <typeparam name ="TPrimaryKey">实体主键的类型</ typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRoot<TPrimaryKey> : AuditedAggregateRoot<TPrimaryKey>, IFullAudited<TPrimaryKey> where TPrimaryKey:struct
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
    [Serializable]
    public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAudited
    {
        public virtual bool IsDeleted { get; set; }

        public virtual string DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
