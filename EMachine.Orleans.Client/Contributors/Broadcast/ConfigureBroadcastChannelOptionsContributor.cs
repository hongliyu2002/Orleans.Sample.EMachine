using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.BroadcastChannel;

namespace EMachine.Orleans.Client.Contributors;

internal sealed class ConfigureBroadcastChannelOptionsContributor : ConfigureOptionsContributorBase<BroadcastChannelOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Client:BroadcastChannel";
}
