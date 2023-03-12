using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EMachine.Orleans.Server.Providers.Redis;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddOrleansRedisClustering(this IServiceCollection services, RedisClusteringOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisClustering(clustering =>
                                                                          {
                                                                              clustering.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                          });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisGrainDirectory(this IServiceCollection services, RedisGrainDirectoryOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisGrainDirectoryAsDefault(grainDirectory =>
                                                                                       {
                                                                                           grainDirectory.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                                       });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisReminder(this IServiceCollection services, RedisReminderOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisReminderService(reminder =>
                                                                               {
                                                                                   reminder.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                               });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisPersistence(this IServiceCollection services, RedisPersistenceOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       foreach (var connectionStringName in options.ConnectionStringNames)
                                       {
                                           if (options.ConnectionStrings.TryGetValue(connectionStringName, out var connectionString))
                                           {
                                               siloBuilder.AddRedisGrainStorage(connectionStringName, persistence =>
                                                                                                      {
                                                                                                          persistence.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                                                      });
                                           }
                                       }
                                   });
    }
}
