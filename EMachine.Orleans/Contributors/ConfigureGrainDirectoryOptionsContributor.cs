using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureGrainDirectoryOptionsContributor : ConfigureOptionsContributorBase<GrainDirectoryOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:GrainDirectory";
}
