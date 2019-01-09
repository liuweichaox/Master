using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存储修改实体信息的删除者和删除时间
    /// </summary>
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// 删除此实体的用户
        /// </summary>
        long? DeleterUserId { get; set; }
    }
}
