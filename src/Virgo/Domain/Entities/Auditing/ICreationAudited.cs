using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 创建审计属性接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface ICreationAudited<TPrimaryKey> : IHasCreationTime where TPrimaryKey : struct
    {
        /// <summary>
        /// 创建此实体的用户
        /// </summary>
        TPrimaryKey? CreatorUserId { get; set; }
    }
    /// <summary>
    /// 创建审计属性接口-主键为String类型
    /// </summary>
    public interface ICreationAudited : IHasCreationTime
    {
        string CreatorUserId { get; set; }
    }
}
