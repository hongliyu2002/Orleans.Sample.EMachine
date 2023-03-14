using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Server.Providers.InMemory.Contributors;

internal sealed class ConfigureInMemoryReminderOptionsContributor : ConfigureOptionsContributorBase<InMemoryReminderOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:InMemory:Reminder";
}
