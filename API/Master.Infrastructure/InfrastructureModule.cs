using System.Reflection;

namespace Master.Infrastructure
{
    public static class InfrastructureModule
    {
        public static readonly Assembly Assembly = typeof(InfrastructureModule).Assembly;
    }
}