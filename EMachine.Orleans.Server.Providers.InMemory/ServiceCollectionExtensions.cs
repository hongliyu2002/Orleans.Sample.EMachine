using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server.Providers.InMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrleansInMemoryClustering(this IServiceCollection services, InMemoryClusteringOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                   });
    }

    public static IServiceCollection AddOrleansInMemoryReminder(this IServiceCollection services, InMemoryReminderOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                   });
    }

    public static IServiceCollection AddOrleansInMemoryPersistence(this IServiceCollection services, InMemoryPersistenceOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                   });
    }
}
