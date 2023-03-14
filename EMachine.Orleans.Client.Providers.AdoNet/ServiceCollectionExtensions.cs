using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Client.Providers.AdoNet;

public static class ServiceCollectionExtensions
{
    private const string NotSupportsMessage = "Database provider does not support.";

    public static IServiceCollection AddOrleansClientAdoNetClustering(this IServiceCollection services, AdoNetClusteringOptions options)
    {
        if (!options.FeatureEnabled)
        {
            return services;
        }
        return services.AddOrleansClient(builder =>
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
}
