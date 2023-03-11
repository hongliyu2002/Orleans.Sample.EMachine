using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisGrainDirectoryOptionsContributor : ConfigureOptionsContributorBase<RedisGrainDirectoryOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:GrainDirectory";
}
