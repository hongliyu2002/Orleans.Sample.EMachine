using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureMemoryGrainStorageOptionsContributor : ConfigureOptionsContributorBase<MemoryGrainStorageOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:MemoryGrainStorage";
}
