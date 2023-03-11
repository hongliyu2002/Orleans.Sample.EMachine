using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server.Providers.AdoNet;

[PublicAPI]
public static class HostBuilderExtensions
{
    public static IHostBuilder AddOrleansAdoNet(this IHostBuilder builder)
    {
        return builder.AddOrleansAdoNet((_, _) =>
                                               {
                                               });
    }

    public static IHostBuilder AddOrleansAdoNet(this IHostBuilder builder, Action<ISiloBuilder> configureAction)
    {
        return builder.AddOrleansAdoNet((_, silo) =>
                                               {
                                                   configureAction.Invoke(silo);
                                               });
    }

    public static IHostBuilder AddOrleansAdoNet(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder> configureAction)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      configureAction.Invoke(context, silo);
                                  });
    }
}
