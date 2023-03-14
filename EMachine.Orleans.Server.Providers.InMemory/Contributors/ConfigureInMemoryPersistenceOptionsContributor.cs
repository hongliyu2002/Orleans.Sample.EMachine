using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Server.Providers.InMemory.Contributors;

internal sealed class ConfigureInMemoryPersistenceOptionsContributor : ConfigureOptionsContributorBase<InMemoryPersistenceOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:InMemory:Persistence";
}
