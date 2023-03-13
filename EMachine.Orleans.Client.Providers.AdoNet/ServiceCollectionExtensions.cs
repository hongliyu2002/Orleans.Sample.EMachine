using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EMachine.Orleans.Client.Providers.AdoNet;

public static class ServiceCollectionExtensions
{
    private const string NotSupportsMessage = "Database provider does not support.";

    public static IServiceCollection AddOrleansClientAdoNetClustering(this IServiceCollection services, AdoNetClusteringOptions options)
    {
        return services.AddOrleansClient(builder =>
                                         {
                                             if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                             {
                                                 builder.UseAdoNetClustering(clustering =>
                                                                             {
                                                                                 clustering.ConnectionString = connectionString;
                                                                                 clustering.Invariant = options.DatabaseProvider switch
                                                                                                        {
                                                                                                            AdoNetDatabaseProvider.SqlServer => AdoNetInvariants.InvariantNameSqlServer,
                                                                                                            AdoNetDatabaseProvider.PostgreSql => AdoNetInvariants.InvariantNamePostgreSql,
                                                                                                            AdoNetDatabaseProvider.MySql => AdoNetInvariants.InvariantNameMySql,
                                                                                                            AdoNetDatabaseProvider.Oracle => AdoNetInvariants.InvariantNameOracleDatabase,
                                                                                                            _ => throw new ArgumentOutOfRangeException(nameof(options.DatabaseProvider), NotSupportsMessage)
                                                                                                        };
                                                                             });
                                             }
                                         });
    }
}
