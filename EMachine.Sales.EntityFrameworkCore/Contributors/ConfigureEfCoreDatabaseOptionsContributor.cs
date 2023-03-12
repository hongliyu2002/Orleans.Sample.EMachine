using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.EntityFrameworkCore.Contributors;

internal sealed class ConfigureEfCoreDatabaseOptionsContributor : ConfigureOptionsContributorBase<EfCoreDatabaseOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Sales:Database";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, EfCoreDatabaseOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(EfCoreDatabaseOptions)", services =>
                        {
                            return services.Configure<EfCoreDatabaseOptions>(options =>
                                                                             {
                                                                                 options.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                             });
                        });
    }
}
