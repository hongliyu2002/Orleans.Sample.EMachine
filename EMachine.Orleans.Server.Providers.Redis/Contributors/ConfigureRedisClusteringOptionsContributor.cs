using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisClusteringOptionsContributor : ConfigureOptionsContributorBase<RedisClusteringOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Clustering";
}
