using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Machine.Orleans.Server.Providers.AdoNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class AdoNetPersistenceHealthChecksContributor : IHealthChecksContributor
{
    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<AdoNetPersistenceOptions>();
        options.ConnectionStrings = context.Services.GetObject<ConnectionStrings>();
        foreach (var connectionStringName in options.ConnectionStringNames)
        {
            if (options.ConnectionStrings.TryGetValue(connectionStringName, out var connectionString))
            {
                switch (options.DatabaseProvider)
                {
                    case AdoNetDatabaseProvider.SqlServer:
                        builder.AddSqlServer(connectionString, "SELECT 1;", null, $"AdoNetPersistence-{connectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                    case AdoNetDatabaseProvider.PostgreSql:
                        builder.AddNpgSql(connectionString, "SELECT 1;", null, $"AdoNetPersistence-{connectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                    case AdoNetDatabaseProvider.MySql:
                        builder.AddMySql(connectionString, $"AdoNetPersistence-{connectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                    case AdoNetDatabaseProvider.Oracle:
                        builder.AddOracle(connectionString, "select * from v$version", null, $"AdoNetPersistence-{connectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                }
            }
        }
    }
}
