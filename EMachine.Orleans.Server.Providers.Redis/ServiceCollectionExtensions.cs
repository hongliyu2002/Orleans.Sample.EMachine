using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EMachine.Orleans.Server.Providers.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansRedisGrainDirectory(this IServiceCollection services)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       var redisOptions = services.GetOptions<RedisGrainDirectoryOptions>();
                                       redisOptions.ConnectionStrings = services.GetObject<ConnectionStrings>();
                                       if (redisOptions.ConnectionStrings.TryGetValue(redisOptions.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisGrainDirectoryAsDefault(options => options.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisClustering(this IServiceCollection services)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       var redisOptions = services.GetOptions<RedisClusteringOptions>();
                                       redisOptions.ConnectionStrings = services.GetObject<ConnectionStrings>();
                                       if (redisOptions.ConnectionStrings.TryGetValue(redisOptions.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisClustering(options => options.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisReminder(this IServiceCollection services)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       var redisOptions = services.GetOptions<RedisReminderTableOptions>();
                                       redisOptions.ConnectionStrings = services.GetObject<ConnectionStrings>();
                                       if (redisOptions.ConnectionStrings.TryGetValue(redisOptions.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseRedisReminderService(options => options.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansRedisStorage(this IServiceCollection services)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       var redisOptions = services.GetOptions<RedisStorageOptions>();
                                       redisOptions.ConnectionStrings = services.GetObject<ConnectionStrings>();
                                       foreach (var connectionStringName in redisOptions.ConnectionStringNames)
                                       {
                                           if (redisOptions.ConnectionStrings.TryGetValue(connectionStringName, out var connectionString))
                                           {
                                               siloBuilder.AddRedisGrainStorage(connectionStringName, options => options.ConfigurationOptions = ConfigurationOptions.Parse(connectionString));
                                           }
                                       }
                                   });
    }
}
