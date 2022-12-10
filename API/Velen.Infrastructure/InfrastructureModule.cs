using System.Reflection;

namespace Velen.Infrastructure
{
    public static class InfrastructureModule
    {
        public static readonly Assembly Assembly = typeof(InfrastructureModule).Assembly;
    }
}