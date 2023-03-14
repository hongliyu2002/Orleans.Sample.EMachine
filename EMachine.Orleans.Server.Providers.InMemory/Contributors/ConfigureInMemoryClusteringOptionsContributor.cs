using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Server.Providers.InMemory.Contributors;

internal sealed class ConfigureInMemoryClusteringOptionsContributor : ConfigureOptionsContributorBase<InMemoryClusteringOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:InMemory:Clustering";
}
