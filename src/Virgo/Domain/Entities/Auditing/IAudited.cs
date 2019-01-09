using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Entities.Auditing
{
    /// <summary>
    /// 此接口由必须审核的实体实现
    /// 对象（保存/更新）时自动设置相关属性
    /// </summary>
    public interface IAudited : ICreationAudited, IModificationAudited
    {

    }    
}
