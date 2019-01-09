using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口存实现（保存/更新）审计相关属性
    /// </summary>
    public interface IAudited<TPrimaryKey> : ICreationAudited<TPrimaryKey>, IModificationAudited<TPrimaryKey> where TPrimaryKey : struct
    {

    }
    public interface IAudited : ICreationAudited, IModificationAudited
    {

    }
}
