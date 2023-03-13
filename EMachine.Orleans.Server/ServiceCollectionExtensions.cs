using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

namespace EMachine.Orleans.Server;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansServer(this IServiceCollection services, ServerOptions options)
    {
        return services.AddOrleans(builder =>
                                   {
                                   });
    }
}
