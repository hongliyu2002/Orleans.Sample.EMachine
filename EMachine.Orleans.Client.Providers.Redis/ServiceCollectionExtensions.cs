using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EMachine.Orleans.Client.Providers.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansRedisClustering(this IServiceCollection services, RedisClusteringOptions options)
    {
        return services.AddOrleansClient(builder =>
                                         {
                                             if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                             {
                                                 builder.UseRedisClustering(clustering =>
                                                                            {
                                                                                clustering.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                            });
                                             }
                                         });
    }
}
