using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureLeaseBasedQueueBalancerOptionsContributor : ConfigureOptionsContributorBase<LeaseBasedQueueBalancerOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:LeaseBasedQueueBalancer";
}
