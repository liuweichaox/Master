using System.Reflection;

namespace Master.Application;

public class ApplicationModule
{
    public static readonly Assembly Assembly = typeof(ApplicationModule).Assembly;
}