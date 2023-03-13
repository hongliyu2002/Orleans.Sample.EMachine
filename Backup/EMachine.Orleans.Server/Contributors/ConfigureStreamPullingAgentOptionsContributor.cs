using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureStreamPullingAgentOptionsContributor : ConfigureOptionsContributorBase<StreamPullingAgentOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:StreamPullingAgent";
}
