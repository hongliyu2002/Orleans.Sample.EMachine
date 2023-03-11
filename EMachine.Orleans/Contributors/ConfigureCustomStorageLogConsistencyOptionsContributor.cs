using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureCustomStorageLogConsistencyOptionsContributor : ConfigureOptionsContributorBase<CustomStorageLogConsistencyOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:CustomStorageLogConsistency";
}
