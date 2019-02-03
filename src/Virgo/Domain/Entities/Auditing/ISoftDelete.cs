using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 软删除属性接口
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 用于将实体标记为"已删除"
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
