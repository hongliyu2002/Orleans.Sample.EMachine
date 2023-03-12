using Machine.Orleans.Server.Providers.AdoNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Server.Providers.AdoNet;

public static class ServiceCollectionExtensions
{
    private const string ProviderNotSupport = "Database provider does not support.";

    public static IServiceCollection AddOrleansAdoNetClustering(this IServiceCollection services, AdoNetClusteringOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseAdoNetClustering(clustering =>
                                                                           {
                                                                               clustering.ConnectionString = connectionString;
                                                                               switch (options.DatabaseProvider)
                                                                               {
                                                                                   case AdoNetDatabaseProvider.SqlServer:
                                                                                       clustering.Invariant = AdoNetInvariants.InvariantNameSqlServer;
                                                                                       break;
                                                                                   case AdoNetDatabaseProvider.PostgreSql:
                                                                                       clustering.Invariant = AdoNetInvariants.InvariantNamePostgreSql;
                                                                                       break;
                                                                                   case AdoNetDatabaseProvider.MySql:
                                                                                       clustering.Invariant = AdoNetInvariants.InvariantNameMySql;
                                                                                       break;
                                                                                   case AdoNetDatabaseProvider.Oracle:
                                                                                       clustering.Invariant = AdoNetInvariants.InvariantNameOracleDatabase;
                                                                                       break;
                                                                                   default:
                                                                                       throw new ArgumentOutOfRangeException(nameof(options.DatabaseProvider), ProviderNotSupport);
                                                                               }
                                                                           });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansAdoNetReminder(this IServiceCollection services, AdoNetReminderOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                       {
                                           siloBuilder.UseAdoNetReminderService(reminder =>
                                                                                {
                                                                                    reminder.ConnectionString = connectionString;
                                                                                    switch (options.DatabaseProvider)
                                                                                    {
                                                                                        case AdoNetDatabaseProvider.SqlServer:
                                                                                            reminder.Invariant = AdoNetInvariants.InvariantNameSqlServer;
                                                                                            break;
                                                                                        case AdoNetDatabaseProvider.PostgreSql:
                                                                                            reminder.Invariant = AdoNetInvariants.InvariantNamePostgreSql;
                                                                                            break;
                                                                                        case AdoNetDatabaseProvider.MySql:
                                                                                            reminder.Invariant = AdoNetInvariants.InvariantNameMySql;
                                                                                            break;
                                                                                        case AdoNetDatabaseProvider.Oracle:
                                                                                            reminder.Invariant = AdoNetInvariants.InvariantNameOracleDatabase;
                                                                                            break;
                                                                                        default:
                                                                                            throw new ArgumentOutOfRangeException(nameof(options.DatabaseProvider), ProviderNotSupport);
                                                                                    }
                                                                                });
                                       }
                                   });
    }

    public static IServiceCollection AddOrleansAdoNetPersistence(this IServiceCollection services, AdoNetPersistenceOptions options)
    {
        return services.AddOrleans(siloBuilder =>
                                   {
                                       foreach (var connectionStringName in options.ConnectionStringNames)
                                       {
                                           if (options.ConnectionStrings.TryGetValue(connectionStringName, out var connectionString))
                                           {
                                               siloBuilder.AddAdoNetGrainStorage(connectionStringName, persistence =>
                                                                                                       {
                                                                                                           persistence.ConnectionString = connectionString;
                                                                                                           switch (options.DatabaseProvider)
                                                                                                           {
                                                                                                               case AdoNetDatabaseProvider.SqlServer:
                                                                                                                   persistence.Invariant = AdoNetInvariants.InvariantNameSqlServer;
                                                                                                                   break;
                                                                                                               case AdoNetDatabaseProvider.PostgreSql:
                                                                                                                   persistence.Invariant = AdoNetInvariants.InvariantNamePostgreSql;
                                                                                                                   break;
                                                                                                               case AdoNetDatabaseProvider.MySql:
                                                                                                                   persistence.Invariant = AdoNetInvariants.InvariantNameMySql;
                                                                                                                   break;
                                                                                                               case AdoNetDatabaseProvider.Oracle:
                                                                                                                   persistence.Invariant = AdoNetInvariants.InvariantNameOracleDatabase;
                                                                                                                   break;
                                                                                                               default:
                                                                                                                   throw new ArgumentOutOfRangeException(nameof(options.DatabaseProvider), ProviderNotSupport);
                                                                                                           }
                                                                                                       });
                                           }
                                       }
                                   });
    }
}
