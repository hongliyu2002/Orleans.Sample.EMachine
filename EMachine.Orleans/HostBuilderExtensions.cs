using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans;

[PublicAPI]
public static class HostBuilderExtensions
{
    public static IHostBuilder AddOrleans(this IHostBuilder builder)
    {
        return builder.AddOrleans((_, _) =>
                                  {
                                  });
    }

    public static IHostBuilder AddOrleans(this IHostBuilder builder, Action<ISiloBuilder> configureAction)
    {
        return builder.AddOrleans((_, silo) =>
                                  {
                                      configureAction.Invoke(silo);
                                  });
    }

    public static IHostBuilder AddOrleans(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder> configureAction)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      silo.AddAdoNetGrainStorage("Test");
                                      configureAction.Invoke(context, silo);
                                  });
    }
}
