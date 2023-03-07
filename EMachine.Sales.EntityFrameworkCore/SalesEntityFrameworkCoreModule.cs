using EMachine.Sales.EntityFrameworkCore.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.Persistence;
using Fluxera.Extensions.Hosting.Modules.Persistence.EntityFrameworkCore;
using JetBrains.Annotations;

namespace EMachine.Sales.EntityFrameworkCore;

[PublicAPI]
[DependsOn<EntityFrameworkCorePersistenceModule>]
[DependsOn<ConfigurationModule>]
public class SalesEntityFrameworkCoreModule : ConfigureServicesModule
{
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddRepositoryContributor<RepositoryContributor>();
    }
}
