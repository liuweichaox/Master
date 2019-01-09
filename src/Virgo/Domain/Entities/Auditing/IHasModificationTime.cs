using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 此接口由希望存储修改信息的实体最后修改时间
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// 此实体的上次修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}
