using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存实现（删除）审计相关属性
    /// </summary>
    public interface IDeletionAudited<TPrimaryKey> : IHasDeletionTime where TPrimaryKey : struct
    {
        /// <summary>
        /// 删除此实体的用户
        /// </summary>
        TPrimaryKey? DeleterUserId { get; set; }
    }
    public interface IDeletionAudited : IHasDeletionTime
    {
        string DeleterUserId { get; set; }
    }
}
