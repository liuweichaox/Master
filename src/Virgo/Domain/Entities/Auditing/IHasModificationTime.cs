using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 修改时间属性接口
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// 此实体的上次修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}
