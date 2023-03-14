using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EMachine.Orleans.Server.Providers.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansRedisClustering(this IServiceCollection services, RedisClusteringOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
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

    public static IServiceCollection AddOrleansRedisGrainDirectory(this IServiceCollection services, RedisGrainDirectoryOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           builder.UseRedisGrainDirectoryAsDefault(grainDirectory =>
                                                                                   {
                                                                                       grainDirectory.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                                   });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisReminder(this IServiceCollection services, RedisReminderOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           builder.UseRedisReminderService(reminder =>
                                                                           {
                                                                               reminder.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                           });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisPersistence(this IServiceCollection services, RedisPersistenceOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                       foreach (var name in options.ConnectionStringNames)
                                       {
                                           if (options.ConnectionStrings.TryGetValue(name, out var connectionString))
                                           {
                                               builder.AddRedisGrainStorage(name, persistence =>
                                                                                  {
                                                                                      persistence.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                                                                                  });
                                           }
                                       }
                                   });
    }
}
