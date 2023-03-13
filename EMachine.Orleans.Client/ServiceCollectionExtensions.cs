using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansClient(this IServiceCollection services, ClientOptions options)
    {
        return services.AddOrleansClient(builder =>
                                         {
                                         });
    }
}
