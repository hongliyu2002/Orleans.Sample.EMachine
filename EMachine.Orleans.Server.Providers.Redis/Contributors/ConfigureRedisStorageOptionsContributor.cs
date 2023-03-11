using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Persistence;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisStorageOptionsContributor : ConfigureOptionsContributorBase<RedisStorageOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Storage";
}
