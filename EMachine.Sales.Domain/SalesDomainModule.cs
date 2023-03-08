using EMachine.Sales.Domain.Repositories;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.Domain;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EMachine.Sales.Domain;

[PublicAPI]
[DependsOn<DomainModule>]
[DependsOn<ConfigurationModule>]
public sealed class SalesDomainModule : ConfigureServicesModule
{
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        // Add repositories.
        context.Log("AddRepositories", services =>
                                       {
                                           services.TryAddTransient<ISnackRepository, SnackRepository>();
                                           services.TryAddTransient<ISnackMachineRepository, SnackMachineRepository>();
                                       });
    }
}
