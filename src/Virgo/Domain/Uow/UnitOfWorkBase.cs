using Autofac.Extras.IocManager;
using System;
using System.Data;

namespace Virgo.Domain.Uow
{
    /// <summary>
    /// 工作单元基类，所有工作单元类继承自此类
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork, ILifetimeScopeDependency
    {
        /// <summary>
        /// 当前资源是否已释放
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction Transaction { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        public IDbConnection Connection { get; }
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
        /// <see cref="BeginTransaction(IsolationLevel?)"/>方法是否被调用过
        /// </summary>
        private bool _isBeginCalled;
        /// <summary>
        /// <see cref="Commit"/>方法是否被调用过
        /// </summary>
        private bool _isCommitCalled;
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
        public UnitOfWorkBase()
        {
            Connection = CreateConnection();
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
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
        private void IsCommitCalled()
        {
            if (_isCommitCalled)
            {
                throw new Exception("请勿重复提交");
            }
            _isCommitCalled = true;
        }
        public abstract IDbConnection CreateConnection();
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        public void BeginTransaction(IsolationLevel? isolationLevel = null)
        {
            IsBeginCalled();
            Transaction = Connection.BeginTransaction();
            if (isolationLevel.HasValue)
            {
                Transaction = Connection.BeginTransaction(isolationLevel.Value);
            }
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            IsCommitCalled();
            try
            {
                Transaction.Commit();
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
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
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
            Transaction.Dispose();
            Connection.Dispose();
            IsDisposed = true;
            if (!_succeed)
            {
                OnFailed(_exception);
            }
            OnDisposed();
        }
    }
}
