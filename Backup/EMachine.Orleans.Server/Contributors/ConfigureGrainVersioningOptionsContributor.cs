using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureGrainVersioningOptionsContributor : ConfigureOptionsContributorBase<GrainVersioningOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:GrainVersioning";
}
