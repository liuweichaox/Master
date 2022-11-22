using System;
using Autofac.Core.Resolving.Pipeline;
using Virgo.DependencyInjection;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// <see cref="IUnitOfWork"/>AOP模式执行
    /// </summary>
    public class UnitOfWorkInterceptor : IResolveMiddleware, ITransientDependency
    {
        private readonly IUnitOfWork _unitOfWork;
        public PipelinePhase Phase => PipelinePhase.RegistrationPipelineStart;
        public UnitOfWorkInterceptor()
        {
        }
        public UnitOfWorkInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            // var uowAttr = context.Instance.GetCustomAttribute(typeof(UnitOfWorkAttribute)) as UnitOfWorkAttribute;
            // if (invocation.MethodInvocationTarget.IsDefined(typeof(UnitOfWorkAttribute), true))
            // {
            //     try
            //     {
            //         _unitOfWork.BeginTransaction();
            //         if (uowAttr.IsolationLevel.HasValue)
            //         {
            //             _unitOfWork.BeginTransaction(uowAttr.IsolationLevel.Value);
            //         }
            //         next(context);
            //         _unitOfWork.Commit();
            //     }
            //     catch (System.Exception)
            //     {
            //         _unitOfWork.Rollback();
            //     }
            // }
            // next(context);
            // return;
        }
    }
}
