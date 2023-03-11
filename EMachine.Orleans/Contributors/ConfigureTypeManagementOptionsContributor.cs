using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureTypeManagementOptionsContributor : ConfigureOptionsContributorBase<TypeManagementOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:TypeManagement";
}
