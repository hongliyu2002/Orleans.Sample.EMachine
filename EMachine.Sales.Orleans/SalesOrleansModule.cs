using EMachine.Orleans.Server;
using EMachine.Sales.Orleans.EntityFrameworkCore;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Sales.Orleans;

[PublicAPI]
[DependsOn<SalesOrleansEntityFrameworkCoreModule>]
[DependsOn<OrleansServerModule>]
[DependsOn<ConfigurationModule>]
public class SalesOrleansModule : ConfigureServicesModule
{

}
