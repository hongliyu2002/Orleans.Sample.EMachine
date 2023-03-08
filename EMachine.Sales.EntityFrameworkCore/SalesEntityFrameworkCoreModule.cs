using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.EntityFrameworkCore;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class SalesEntityFrameworkCoreModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddDbContextPool<SalesDbContext>(options =>
                                                          {
                                                              var connectionString = context.Configuration.GetConnectionString("SalesDB") ?? "Data Source=Sales.db";
                                                              options.UseSqlite(connectionString, sqlite =>
                                                                                                  {
                                                                                                      sqlite.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                                                                                                  });
                                                          });
    }
}
