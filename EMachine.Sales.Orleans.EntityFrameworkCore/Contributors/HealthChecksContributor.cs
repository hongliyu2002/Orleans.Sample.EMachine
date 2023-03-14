using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Sales.Orleans.EntityFrameworkCore.Contributors;

internal sealed class HealthChecksContributor : IHealthChecksContributor
{
    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<SalesDbOptions>();
        if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out _))
        {
            builder.AddDbContextCheck<SalesDbContext>(options.ConnectionStringName, HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
        }
    }
}
