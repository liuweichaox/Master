using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存实现（保存/更新/删除）审计相关属性
    /// </summary>
    public interface IFullAudited<TPrimaryKey> : IAudited<TPrimaryKey>, IDeletionAudited<TPrimaryKey> where TPrimaryKey:struct
    {

    }

    public interface IFullAudited : IAudited, IDeletionAudited
    {

    }
}
