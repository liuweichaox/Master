using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 此类可用于简化实现<see cref=“iaudited”/>
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体的主键类型</typeparam>
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
    [Serializable]
    public abstract class AuditedEntity : CreationAuditedEntity, IAudited
    {
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual string LastModifierUserId { get; set; }
    }
}
