using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EMachine.Orleans.Server.Providers.Redis;

[PublicAPI]
public static class HostBuilderExtensions
{
    public static IHostBuilder AddOrleansRedisGrainDirectory(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder>? configureAction = null)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      var redisGrainDirectoryOptions = silo.Services.GetOptions<RedisGrainDirectoryOptions>();
                                      redisGrainDirectoryOptions.ConnectionStrings = silo.Services.GetObject<ConnectionStrings>();
                                      silo.UseRedisGrainDirectoryAsDefault(options => options.ConfigurationOptions = ConfigurationOptions.Parse(redisGrainDirectoryOptions.ConnectionStrings[redisGrainDirectoryOptions.ConnectionStringName]));
                                      configureAction?.Invoke(context, silo);
                                  });
    }

    public static IHostBuilder AddOrleansRedisClustering(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder>? configureAction = null)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      var redisClusteringOptions = silo.Services.GetOptions<RedisClusteringOptions>();
                                      redisClusteringOptions.ConnectionStrings = silo.Services.GetObject<ConnectionStrings>();
                                      silo.UseRedisClustering(options => options.ConfigurationOptions = ConfigurationOptions.Parse(redisClusteringOptions.ConnectionStrings[redisClusteringOptions.ConnectionStringName]));
                                      configureAction?.Invoke(context, silo);
                                  });
    }

    public static IHostBuilder AddOrleansRedisGrainStorage(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder>? configureAction = null)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      var redisStorageOptions = silo.Services.GetOptions<RedisStorageOptions>();
                                      redisStorageOptions.ConnectionStrings = silo.Services.GetObject<ConnectionStrings>();
                                      foreach (var connectionStringName in redisStorageOptions.ConnectionStringNames)
                                      {
                                          silo.AddRedisGrainStorage(connectionStringName, options => options.ConfigurationOptions = ConfigurationOptions.Parse(redisStorageOptions.ConnectionStrings[connectionStringName]));
                                      }
                                      configureAction?.Invoke(context, silo);
                                  });
    }

    public static IHostBuilder AddOrleansRedisReminder(this IHostBuilder builder, Action<HostBuilderContext, ISiloBuilder>? configureAction = null)
    {
        return builder.UseOrleans((context, silo) =>
                                  {
                                      var redisReminderTableOptions = silo.Services.GetOptions<RedisReminderTableOptions>();
                                      redisReminderTableOptions.ConnectionStrings = silo.Services.GetObject<ConnectionStrings>();
                                      silo.UseRedisReminderService(options => options.ConfigurationOptions = ConfigurationOptions.Parse(redisReminderTableOptions.ConnectionStrings[redisReminderTableOptions.ConnectionStringName]));
                                      configureAction?.Invoke(context, silo);
                                  });
    }
}
