using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server.Providers.Redis;

[PublicAPI]
public static class HostBuilderExtensions
{
    public static IHostBuilder AddOrleansRedis(this IHostBuilder builder)
    {
        return builder.AddOrleansRedis((_, _) =>
                                               {
                                               });
    }

    public static IHostBuilder AddOrleansRedis(this IHostBuilder builder, Action<ISiloBuilder> configureAction)
    {
        return builder.AddOrleansRedis((_, silo) =>
                                               {
                                                   configureAction.Invoke(silo);
                                               });
    }

    public static IHostBuilder AddOrleansRedis(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder> configureAction)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      configureAction.Invoke(context, silo);
                                  });
    }
}
