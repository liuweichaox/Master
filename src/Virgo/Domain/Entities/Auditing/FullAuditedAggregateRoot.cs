using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 实现<see cref =“IFullAudited”/>作为完全审核的聚合根的基类。
    /// </summary>
    /// <typeparam name =“TPrimaryKey”>实体主键的类型</ typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRoot<TPrimaryKey> : AuditedAggregateRoot<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// 此实体是否已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 哪个用户删除了这个实体
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除此实体的时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}
