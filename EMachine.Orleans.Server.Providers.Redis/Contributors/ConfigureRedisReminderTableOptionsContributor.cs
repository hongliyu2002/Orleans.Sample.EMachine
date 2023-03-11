using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisReminderTableOptionsContributor : ConfigureOptionsContributorBase<RedisReminderTableOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:ReminderTable";
}
