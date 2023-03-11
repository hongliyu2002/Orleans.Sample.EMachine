using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureEndpointOptionsContributor : ConfigureOptionsContributorBase<EndpointOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Endpoint";
}
