using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureTypeManagementOptionsContributor : ConfigureOptionsContributorBase<TypeManagementOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:TypeManagement";
}
