using System;
using System.Reflection;
using Autofac.Core.Resolving.Pipeline;
using Metalama.Framework.Aspects;
using Virgo.DependencyInjection;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// <see cref="IUnitOfWork"/>AOP模式执行
    /// </summary>
    public class UnitOfWorkInterceptor : OverrideMethodAspect
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkInterceptor()
        {
        }
        public UnitOfWorkInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override dynamic OverrideMethod()
        {
            dynamic result;
            var uowAttr = meta.Target.GetType().GetCustomAttribute(typeof(UnitOfWorkAttribute)) as UnitOfWorkAttribute;
            if (meta.Target.Method.GetType().IsDefined(typeof(UnitOfWorkAttribute), true))
            {
                try
                {
                    _unitOfWork.BeginTransaction();
                    if (uowAttr.IsolationLevel.HasValue)
                    {
                        _unitOfWork.BeginTransaction(uowAttr.IsolationLevel.Value);
                    }
                    result = meta.Proceed();
                    _unitOfWork.Commit();
                }
                catch (System.Exception)
                {
                    _unitOfWork.Rollback();
                }
            }
            result = meta.Proceed();
            return result;
        }
    }
}
