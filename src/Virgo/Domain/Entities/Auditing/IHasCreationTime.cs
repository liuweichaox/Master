using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存储实体的创建时间属性
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// 该实体的创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
