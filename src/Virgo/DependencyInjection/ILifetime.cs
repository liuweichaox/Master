using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.DependencyInjection
{
    /// <summary>
    ///     Just an interface marking for dependency lifecycle
    ///     <see cref="ILifetimeScopeDependency" />,
    ///     <see cref="ITransientDependency" />,
    ///     <see cref="ISingletonDependency" />
    /// </summary>
    public interface ILifetime { }
}
