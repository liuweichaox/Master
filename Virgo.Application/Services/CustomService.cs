using Virgo.Application.Interfaces;
using Virgo.DependencyInjection;
using Virgo.Domain.Interfaces;

namespace Virgo.Application.Services
{
    /// <summary>
    /// <see cref="ICustomService"/>服务实现类
    /// </summary>
    public class CustomService : ICustomService, ITransientDependency
    {
        private readonly IRepository _repository;
        public CustomService(IRepository repository)
        {
            _repository = repository;
        }
        public bool Call()
        {
            System.Diagnostics.Debug.WriteLine("CustomService Calling");
            return _repository.Call();
        }
    }
}
