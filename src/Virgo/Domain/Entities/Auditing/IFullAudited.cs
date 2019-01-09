using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 希望此接口实现完整审计的实体,（保存/更新/删除）相关属性
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {

    }
}
