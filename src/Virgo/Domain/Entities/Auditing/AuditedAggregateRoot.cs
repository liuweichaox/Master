using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 此类可用于简化实现<see cref =“IAudited”/>的聚合根
    /// </summary>
    /// <typeparam name =“TPrimaryKey”>实体主键的类型</ typeparam>
    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey> : CreationAuditedAggregateRoot<TPrimaryKey>, IAudited
    {
        /// <summary>
        /// 此实体的上次修改日期
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 上次修改此实体的用户
        /// </summary>
        public virtual long? LastModifierUserId { get; set; }
    }
}
