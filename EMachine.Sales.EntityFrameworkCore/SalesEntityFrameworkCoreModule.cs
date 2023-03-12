using EMachine.Sales.EntityFrameworkCore.Contributors;
using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using JetBrains.Annotations;

namespace EMachine.Sales.EntityFrameworkCore;

[PublicAPI]
[DependsOn<HealthChecksModule>]
[DependsOn<DataManagementModule>]
[DependsOn<OpenTelemetryModule>]
[DependsOn<ConfigurationModule>]
public class SalesEntityFrameworkCoreModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureEfCoreDatabaseOptionsContributor>();
        context.Services.AddHealthCheckContributor<HealthChecksContributor>();
        context.Services.AddTracerProviderContributor<TracerProviderContributor>();
    }

    /// <inheritdoc />
    public override void PostConfigureServices(IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<EfCoreDatabaseOptions>();
        options.ConnectionStrings = context.Services.GetObject<ConnectionStrings>();
        context.Log("AddDbContextPoolForSales", services => services.AddDbContextPoolForSales(options));
    }
}
