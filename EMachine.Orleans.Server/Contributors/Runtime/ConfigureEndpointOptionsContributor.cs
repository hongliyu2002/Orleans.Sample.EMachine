using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureEndpointOptionsContributor : ConfigureOptionsContributorBase<EndpointOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:Runtime:Endpoint";
}
