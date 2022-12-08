using System.Reflection;

namespace Velen.Infrastructure
{
    internal static class InfrastructureModule
    {
        public static readonly Assembly Assembly = typeof(InfrastructureModule).Assembly;
    }
}