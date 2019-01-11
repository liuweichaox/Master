using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存实现（保存）审计相关属性
    /// </summary>
    public interface ICreationAudited<TPrimaryKey> : IHasCreationTime where TPrimaryKey : struct
    {
        /// <summary>
        /// 创建此实体的用户
        /// </summary>
        TPrimaryKey? CreatorUserId { get; set; }
    }
    public interface ICreationAudited : IHasCreationTime
    {
        string CreatorUserId { get; set; }
    }
}
