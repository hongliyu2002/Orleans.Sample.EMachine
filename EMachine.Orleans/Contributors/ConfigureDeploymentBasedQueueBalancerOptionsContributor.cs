using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureDeploymentBasedQueueBalancerOptionsContributor : ConfigureOptionsContributorBase<DeploymentBasedQueueBalancerOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:DeploymentBasedQueueBalancer";
}
