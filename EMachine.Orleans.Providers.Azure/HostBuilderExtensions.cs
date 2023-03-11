using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Providers.Azure;

[PublicAPI]
public static class HostBuilderExtensions
{
    public static IHostBuilder AddOrleansAzureProvider(this IHostBuilder builder)
    {
        return builder.AddOrleansAzureProvider((_, _) =>
                                               {
                                               });
    }

    public static IHostBuilder AddOrleansAzureProvider(this IHostBuilder builder, Action<ISiloBuilder> configureAction)
    {
        return builder.AddOrleansAzureProvider((_, silo) =>
                                               {
                                                   configureAction.Invoke(silo);
                                               });
    }

    public static IHostBuilder AddOrleansAzureProvider(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder> configureAction)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      configureAction.Invoke(context, silo);
                                  });
    }
}
