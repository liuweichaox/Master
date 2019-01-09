using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 该类可用于简化实现<see cref =“ICreationAudited”/>
    /// </summary>
    /// <typeparam name =“TPrimaryKey”>实体主键的类型</ typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        /// <summary>
        /// 该实体的创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// 这个实体的创造者
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreationAuditedEntity()
        {
            CreationTime = DateTime.Now;
        }
    }
}
