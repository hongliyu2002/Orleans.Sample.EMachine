using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
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
        foreach (var description in options.ConnectionStringDescriptions)
        {
            if (options.ConnectionStrings.TryGetValue(description.ConnectionStringName, out var connectionString))
            {
                switch (description.DbProvider)
                {
                    case AdoNetDbProvider.SqlServer:
                        builder.AddSqlServer(connectionString, "SELECT 1;", null, $"AdoNetPersistence-{description.ConnectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                    case AdoNetDbProvider.PostgreSQL:
                        builder.AddNpgSql(connectionString, "SELECT 1;", null, $"AdoNetPersistence-{description.ConnectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                    case AdoNetDbProvider.MySQL:
                        builder.AddMySql(connectionString, $"AdoNetPersistence-{description.ConnectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                    case AdoNetDbProvider.Oracle:
                        builder.AddOracle(connectionString, "select * from v$version", null, $"AdoNetPersistence-{description.ConnectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                        break;
                }
            }
        }
    }
}
