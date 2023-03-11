using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server.Providers.Azure;

[PublicAPI]
public static class HostBuilderExtensions
{
    public static IHostBuilder AddOrleansAzure(this IHostBuilder builder)
    {
        return builder.AddOrleansAzure((_, _) =>
                                               {
                                               });
    }

    public static IHostBuilder AddOrleansAzure(this IHostBuilder builder, Action<ISiloBuilder> configureAction)
    {
        return builder.AddOrleansAzure((_, silo) =>
                                               {
                                                   configureAction.Invoke(silo);
                                               });
    }

    public static IHostBuilder AddOrleansAzure(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder> configureAction)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      configureAction.Invoke(context, silo);
                                  });
    }
}
