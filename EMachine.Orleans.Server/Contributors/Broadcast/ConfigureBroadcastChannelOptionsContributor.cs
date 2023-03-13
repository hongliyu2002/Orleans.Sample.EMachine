using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.BroadcastChannel;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureBroadcastChannelOptionsContributor : ConfigureOptionsContributorBase<BroadcastChannelOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:BroadcastChannel";
}
