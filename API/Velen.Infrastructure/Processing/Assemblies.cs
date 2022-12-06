using System.Reflection;

namespace Velen.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(Assemblies).Assembly;
    }
}