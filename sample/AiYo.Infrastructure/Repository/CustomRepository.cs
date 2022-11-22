using Virgo.DependencyInjection;
using Virgo.Domain.Interfaces;
using Virgo.Domain.Models;

namespace Virgo.Infrastructure.Repository
{
    /// <summary>
    /// <see cref="IRepository"/>仓储实现类
    /// </summary>
    public class CustomRepository : ICustomRepository, ITransientDependency
    {
        public bool Call(CustomEntity entity)
        {
            System.Diagnostics.Debug.WriteLine("Repository Calling");
            return true;
        }
    }
}
