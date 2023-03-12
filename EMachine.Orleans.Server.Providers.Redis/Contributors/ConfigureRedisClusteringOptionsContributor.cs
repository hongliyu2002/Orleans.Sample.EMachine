using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisClusteringOptionsContributor : ConfigureOptionsContributorBase<RedisClusteringOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Clustering";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, RedisClusteringOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(RedisClusteringOptions)", services =>
                                                         {
                                                             services.Configure<RedisClusteringOptions>(clustering =>
                                                                                                        {
                                                                                                            clustering.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                        });
                                                         });
    }
}
