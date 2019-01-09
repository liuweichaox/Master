using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存储修改实体信息的创建者和创建时间
    /// </summary>
    public interface ICreationAudited<TKey> : IHasCreationTime where TKey:struct
    {
        /// <summary>
        /// 创建此实体的用户
        /// </summary>
        TKey? CreatorUserId { get; set; }
    }
}
