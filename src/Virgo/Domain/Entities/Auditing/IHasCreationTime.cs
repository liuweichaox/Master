using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 此接口由希望存储修改信息的实体创建时间
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// 该实体的创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
