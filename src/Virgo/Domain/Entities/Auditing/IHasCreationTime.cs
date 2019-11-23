using System;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 创建时间属性接口
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// 该实体的创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
