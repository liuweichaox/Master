using Virgo.DependencyInjection;
using Virgo.Domain.Interfaces;

namespace Virgo.Infrastructure.Repository
{
    /// <summary>
    /// <see cref="IRepository"/>仓储实现类
    /// </summary>
    public class Repository : IRepository, ITransientDependency
    {
        public bool Call()
        {
            System.Diagnostics.Debug.WriteLine("Repository Calling");
            return true;
        }
    }
}
