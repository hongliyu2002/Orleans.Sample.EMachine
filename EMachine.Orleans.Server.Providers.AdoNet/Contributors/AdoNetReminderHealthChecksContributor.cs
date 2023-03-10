using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class AdoNetReminderHealthChecksContributor : IHealthChecksContributor
{
    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<AdoNetReminderOptions>();
        options.ConnectionStrings = context.Services.GetObject<ConnectionStrings>();
        if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
        {
            switch (options.DbProvider)
            {
                case AdoNetDbProvider.SqlServer:
                    builder.AddSqlServer(connectionString, "SELECT 1;", null, "AdoNetReminder", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                    break;
                case AdoNetDbProvider.PostgreSQL:
                    builder.AddNpgSql(connectionString, "SELECT 1;", null, "AdoNetReminder", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                    break;
                case AdoNetDbProvider.MySQL:
                    builder.AddMySql(connectionString, "AdoNetReminder", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                    break;
                case AdoNetDbProvider.Oracle:
                    builder.AddOracle(connectionString, "select * from v$version", null, "AdoNetReminder", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
                    break;
            }
        }
    }
}
