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
                                           siloBuilder.UseRedisClustering(clusteringOptions => clusteringOptions.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisGrainDirectory(this IServiceCollection services, RedisGrainDirectoryOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisGrainDirectoryAsDefault(grainDirectoryOptions => grainDirectoryOptions.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisReminder(this IServiceCollection services, RedisReminderTableOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisReminderService(reminderTableOptions => reminderTableOptions.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisStorage(this IServiceCollection services, RedisStorageOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       foreach (var connectionStringName in options.ConnectionStringNames)
                                       {
                                           if (options.ConnectionStrings.TryGetValue(connectionStringName, out var connectionString))
                                           {
                                               siloBuilder.AddRedisGrainStorage(connectionStringName, storageOptions => storageOptions.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                           }
                                       }
                                   });
    }
}
