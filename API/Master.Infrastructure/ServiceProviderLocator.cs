using Microsoft.Extensions.DependencyInjection;

namespace Master.Infrastructure;

public static class ServiceProviderLocator
{
    private static IServiceProvider _serviceProvider;

    public static void SetProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal static IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
    
    internal static AsyncServiceScope CreateAsyncScope()
    {
        return _serviceProvider.CreateAsyncScope();
    }
}