using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.Persistence;
using Fluxera.Extensions.Hosting.Modules.Persistence.EntityFrameworkCore;
using JetBrains.Annotations;

namespace EMachine.Sales.EntityFrameworkCore;

[PublicAPI]
[DependsOn<SalesDomainModule>]
[DependsOn<EntityFrameworkCorePersistenceModule>]
[DependsOn<ConfigurationModule>]
public class SalesEntityFrameworkCoreModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        // Add the repository contributor for the 'Sales' repository.
        context.Services.AddRepositoryContributor<RepositoryContributor>("Sales");
    }
}
