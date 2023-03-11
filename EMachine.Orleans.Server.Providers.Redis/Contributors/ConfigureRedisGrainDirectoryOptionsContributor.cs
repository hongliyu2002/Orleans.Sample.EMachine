using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisGrainDirectoryOptionsContributor : ConfigureOptionsContributorBase<RedisGrainDirectoryOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:GrainDirectory";
}
