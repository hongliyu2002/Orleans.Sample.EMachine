using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureDeploymentLoadPublisherOptionsContributor : ConfigureOptionsContributorBase<DeploymentLoadPublisherOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:Runtime:DeploymentLoadPublisher";
}
