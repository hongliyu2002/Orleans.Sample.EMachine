using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class ConfigureAdoNetPersistenceOptionsContributor : ConfigureOptionsContributorBase<AdoNetPersistenceOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:AdoNet:Persistence";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, AdoNetPersistenceOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(AdoNetPersistenceOptions)", services =>
                                                           {
                                                               services.Configure<AdoNetPersistenceOptions>(persistence =>
                                                                                                            {
                                                                                                                persistence.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                            });
                                                           });
    }
}
