using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 删除时间/软删除属性接口
    /// </summary>
    public interface IHasDeletionTime : ISoftDelete
    {
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
