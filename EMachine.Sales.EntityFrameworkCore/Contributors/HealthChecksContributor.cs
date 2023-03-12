using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Sales.EntityFrameworkCore.Contributors;

internal sealed class HealthChecksContributor : IHealthChecksContributor
{
    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<EfCoreDatabaseOptions>();
        if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
        {
            builder.AddDbContextCheck<SalesDbContext>(options.ConnectionStringName, HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
        }
    }
}
