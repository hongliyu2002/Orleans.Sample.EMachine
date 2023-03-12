using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisGrainDirectoryOptionsContributor : ConfigureOptionsContributorBase<RedisGrainDirectoryOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:GrainDirectory";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, RedisGrainDirectoryOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(RedisGrainDirectoryOptions)", services =>
                                                             {
                                                                 services.Configure<RedisGrainDirectoryOptions>(grainDirectory =>
                                                                                                                {
                                                                                                                    grainDirectory.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                                });
                                                             });
    }
}
