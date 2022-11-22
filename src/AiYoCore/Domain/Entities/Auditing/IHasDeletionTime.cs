using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 删除时间/软删除属性接口
    /// </summary>
    public interface IHasDeletionTime : ISoftDelete
    {
        /// <summary>
        /// 该实体的删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
