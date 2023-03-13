using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureConnectionOptionsContributor : ConfigureOptionsContributorBase<ConnectionOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Connection";
}
