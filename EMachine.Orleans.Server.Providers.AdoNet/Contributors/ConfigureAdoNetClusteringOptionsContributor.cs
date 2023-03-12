using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class ConfigureAdoNetClusteringOptionsContributor : ConfigureOptionsContributorBase<AdoNetClusteringOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:AdoNet:Clustering";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, AdoNetClusteringOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(AdoNetClusteringOptions)", services =>
                                                          {
                                                              services.Configure<AdoNetClusteringOptions>(clustering =>
                                                                                                          {
                                                                                                              clustering.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                          });
                                                          });
    }
}
