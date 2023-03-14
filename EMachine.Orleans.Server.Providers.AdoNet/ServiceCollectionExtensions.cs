using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server.Providers.AdoNet;

public static class ServiceCollectionExtensions
{
    private const string NotSupportsMessage = "Database provider does not support.";

    public static IServiceCollection AddOrleansAdoNetClustering(this IServiceCollection services, AdoNetClusteringOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           builder.UseAdoNetClustering(clustering =>
                                                                       {
                                                                           clustering.ConnectionString = connectionString;
                                                                           clustering.Invariant = options.DbProvider switch
                                                                                                  {
                                                                                                      AdoNetDbProvider.SqlServer => AdoNetInvariants.InvariantNameSqlServer,
                                                                                                      AdoNetDbProvider.PostgreSQL => AdoNetInvariants.InvariantNamePostgreSql,
                                                                                                      AdoNetDbProvider.MySQL => AdoNetInvariants.InvariantNameMySql,
                                                                                                      AdoNetDbProvider.Oracle => AdoNetInvariants.InvariantNameOracleDatabase,
                                                                                                      _ => throw new ArgumentOutOfRangeException(nameof(options.DbProvider), NotSupportsMessage)
                                                                                                  };
                                                                       });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansAdoNetReminder(this IServiceCollection services, AdoNetReminderOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           builder.UseAdoNetReminderService(reminder =>
                                                                            {
                                                                                reminder.ConnectionString = connectionString;
                                                                                reminder.Invariant = options.DbProvider switch
                                                                                                     {
                                                                                                         AdoNetDbProvider.SqlServer => AdoNetInvariants.InvariantNameSqlServer,
                                                                                                         AdoNetDbProvider.PostgreSQL => AdoNetInvariants.InvariantNamePostgreSql,
                                                                                                         AdoNetDbProvider.MySQL => AdoNetInvariants.InvariantNameMySql,
                                                                                                         AdoNetDbProvider.Oracle => AdoNetInvariants.InvariantNameOracleDatabase,
                                                                                                         _ => throw new ArgumentOutOfRangeException(nameof(options.DbProvider), NotSupportsMessage)
                                                                                                     };
                                                                            });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansAdoNetPersistence(this IServiceCollection services, AdoNetPersistenceOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleans(builder =>
                                   {
                                       foreach (var description in options.ConnectionStringDescriptions)
                                       {
                                           if (options.ConnectionStrings.TryGetValue(description.ConnectionStringName, out var connectionString))
                                           {
                                               builder.AddAdoNetGrainStorage(description.ConnectionStringName, persistence =>
                                                                                                               {
                                                                                                                   persistence.ConnectionString = connectionString;
                                                                                                                   persistence.Invariant = description.DbProvider switch
                                                                                                                                           {
                                                                                                                                               AdoNetDbProvider.SqlServer => AdoNetInvariants.InvariantNameSqlServer,
                                                                                                                                               AdoNetDbProvider.PostgreSQL => AdoNetInvariants.InvariantNamePostgreSql,
                                                                                                                                               AdoNetDbProvider.MySQL => AdoNetInvariants.InvariantNameMySql,
                                                                                                                                               AdoNetDbProvider.Oracle => AdoNetInvariants.InvariantNameOracleDatabase,
                                                                                                                                               _ => throw new ArgumentOutOfRangeException(nameof(description.DbProvider), NotSupportsMessage)
                                                                                                                                           };
                                                                                                               });
                                           }
                                       }
                                   });
    }
}
