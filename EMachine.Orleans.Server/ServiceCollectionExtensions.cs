using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansServer(this IServiceCollection services, ServerOptions options)
    {
        return services.AddOrleans(builder =>
                                   {
                                       foreach (var name in options.BroadcastChannelNames)
                                       {
                                           builder.AddBroadcastChannel(name);
                                       }
                                   });
    }
}
