using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureDevelopmentClusterMembershipOptionsContributor : ConfigureOptionsContributorBase<DevelopmentClusterMembershipOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:DevelopmentClusterMembership";
}
