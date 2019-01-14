using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 用于标准化软删除实体。 软删除实际上并未删除，在数据库中标记为IsDeleted = true，但无法检索到应用程序。
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 用于将实体标记为"已删除"
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
