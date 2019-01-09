using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存储实体的最后修改时间属性
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// 此实体的上次修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}
