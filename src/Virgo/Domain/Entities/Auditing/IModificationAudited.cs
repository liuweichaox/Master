using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 此接口由希望存储修改信息的实体最后修改者和修改时间
    /// </summary>
    public interface IModificationAudited<TPrimaryKey> : IHasModificationTime where TPrimaryKey : struct
    {
        /// <summary>
        /// 此实体的上次修改用户
        /// </summary>
        TPrimaryKey? LastModifierUserId { get; set; }
    }
    public interface IModificationAudited : IHasModificationTime
    {
        string LastModifierUserId { get; set; }
    }
}
