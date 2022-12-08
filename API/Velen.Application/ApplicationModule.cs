using System.Reflection;

namespace Velen.Application;

public class ApplicationModule
{
    public static readonly Assembly Assembly = typeof(ApplicationModule).Assembly;
}