using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.Azure.Contributors;

internal sealed class ConfigureEventHubReceiverOptionsContributor : ConfigureOptionsContributorBase<EventHubReceiverOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:EventHubReceiver";
}
