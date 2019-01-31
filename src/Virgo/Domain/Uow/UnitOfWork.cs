using Autofac.Extras.IocManager;
using System;
using System.Transactions;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 工作单元基类，所有工作单元类继承自此类
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork,ITransientDependency
    {
        /// <summary>
        /// 事务
        /// </summary>
        public TransactionScope Transaction { get; set; }
        /// <summary>
        /// 当前资源是否已释放
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 事务提交成功事件
        /// </summary>
        public event EventHandler Completed;
        /// <summary>
        /// 事务释放时事件
        /// </summary>
        public event EventHandler Disposed;
        /// <summary>
        /// 事务提交失败事件
        /// </summary>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// <see cref="Begin"/>方法是否被调用过
        /// </summary>
        private bool _isBeginCalled;
        /// <summary>
        /// <see cref="Complete"/>方法是否被调用过
        /// </summary>
        private bool _isCompleteCalled;
        /// <summary>
        /// 当前工作单元是否成功完成
        /// </summary>
        private bool _succeed;
        /// <summary>
        /// 工作单元失败异常原因
        /// </summary>
        private Exception _exception;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultOptions"></param>
        public UnitOfWork()
        {
        }
        public IUnitOfWork Begin()
        {
            IsBeginCalled();
            var options = new UnitOfWorkOptions();
            Transaction = CreateTransactionScope(options);
            return this;
        }
        /// <summary>
        /// 根据给定选项开启工作单元
        /// </summary>
        /// <param name="options">工作单元选项<see cref="UnitOfWorkOptions"/></param>
        public IUnitOfWork Begin(UnitOfWorkOptions options)
        {
            IsBeginCalled();
            if (options == null)
                throw new ArgumentNullException("options参数为null");
            Transaction = CreateTransactionScope(options);
            return this;
        }

        /// <summary>
        /// 保存所有更改并提交事务
        /// </summary>
        public void Complete()
        {
            IsCompleteCalled();
            try
            {
                Transaction.Complete();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }
        /// <summary>
        /// 创建事务实例
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public TransactionScope CreateTransactionScope(UnitOfWorkOptions options)
        {
            if (options.Scope.HasValue)
            {
                return new TransactionScope(options.Scope.Value);
            }
            if (options.Scope.HasValue && options.Timeout.HasValue)
            {
                return new TransactionScope(options.Scope.Value, options.Timeout.Value);
            }
            if (options.Scope.HasValue && options.IsolationLevel.HasValue && options.Timeout.HasValue)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = options.IsolationLevel.Value;
                transactionOptions.Timeout = options.Timeout.Value;
                return new TransactionScope(options.Scope.Value, transactionOptions);
            }
            return null;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            Dispose();
            IsDisposed = true;
            if (!_succeed)
            {
                OnFailed(_exception);
            }
            OnDisposed();
        }
        /// <summary>
        /// 工作单元完成事件
        /// </summary>
        private void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 提交失败事件
        /// </summary>
        /// <param name="ex">失败异常</param>
        private void OnFailed(Exception ex)
        {
            Failed?.Invoke(this, new UnitOfWorkFailedEventArgs(ex));
        }
        /// <summary>
        /// 资源释放事件
        /// </summary>
        private void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 工作单元是否已开启
        /// </summary>
        private void IsBeginCalled()
        {
            if (_isBeginCalled)
            {
                throw new Exception("请勿重复调用工作单元启动方法");
            }
            _isBeginCalled = true;
        }
        /// <summary>
        /// 工作单元是否已提交
        /// </summary>
        private void IsCompleteCalled()
        {
            if (_isCompleteCalled)
            {
                throw new Exception("请勿重复提交");
            }
            _isCompleteCalled = true;
        }
    }
}
