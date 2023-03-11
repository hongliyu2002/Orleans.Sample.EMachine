using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureClusterMembershipOptionsContributor : ConfigureOptionsContributorBase<ClusterMembershipOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:ClusterMembership";
}
