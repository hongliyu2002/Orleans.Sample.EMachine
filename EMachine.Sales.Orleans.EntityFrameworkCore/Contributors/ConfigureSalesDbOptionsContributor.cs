using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.Orleans.EntityFrameworkCore.Contributors;

internal sealed class ConfigureSalesDbOptionsContributor : ConfigureOptionsContributorBase<SalesDbOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Sales:Database";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, SalesDbOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(SalesDbOptions)", services =>
                                                 {
                                                     return services.Configure<SalesDbOptions>(options =>
                                                                                               {
                                                                                                   options.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                               });
                                                 });
    }
}
