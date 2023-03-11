using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Client.Contributors;

internal sealed class ConfigureClusterOptionsContributor : ConfigureOptionsContributorBase<ClusterOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Cluster";
}
