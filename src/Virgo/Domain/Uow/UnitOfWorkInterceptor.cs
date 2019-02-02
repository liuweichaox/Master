using Castle.DynamicProxy;
using System.Reflection;

namespace Virgo.Domain.Uow
{
    internal class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkInterceptor()
        {
        }
        public UnitOfWorkInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Intercept(IInvocation invocation)
        {
           var uowAttr = invocation.MethodInvocationTarget.GetCustomAttribute(typeof(UnitOfWorkAttribute)) as UnitOfWorkAttribute;
            if (invocation.MethodInvocationTarget.IsDefined(typeof(UnitOfWorkAttribute), true)&& !uowAttr.IsDisabled)
            {
                using (var uow = _unitOfWork.Begin(uowAttr.CreateOptions()))
                {
                    invocation.Proceed();
                    uow.Complete();
                }
            }
            invocation.Proceed();
            return;
        }
    }
}
