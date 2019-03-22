using System;
using System.Data;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 标记方法是否使用事务提交，如果标记启用事务提交，则所有操作将在打开数据库后一并提交，失败将回滚
    /// </summary>
    /// <remarks>
    /// 如果调用此方法之外已存在一个工作单元，并不会影响，因为他们将会使用同一个事务提交
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public class UnitOfWorkAttribute : Attribute
    {
        public UnitOfWorkAttribute()
        {

        }
        public IsolationLevel?  IsolationLevel { get; set; }
        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            IsolationLevel = isolationLevel;
        }
    }
}
