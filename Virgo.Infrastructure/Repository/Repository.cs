using Virgo.DependencyInjection;
using Virgo.Domain.Interfaces;

namespace Virgo.Infrastructure.Repository
{
    public class Repository : IRepository, ITransientDependency
    {
        public bool Call()
        {
            System.Diagnostics.Debug.WriteLine("Repository Calling");
            return true;
        }
    }
}
