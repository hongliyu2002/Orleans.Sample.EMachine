using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Client.Contributors;

internal sealed class ConfigureGatewayOptionsContributor : ConfigureOptionsContributorBase<GatewayOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Client:Gateway";
}
