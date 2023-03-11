using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureGatewayOptionsContributor : ConfigureOptionsContributorBase<GatewayOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Gateway";
}
