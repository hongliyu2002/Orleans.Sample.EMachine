using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureActivationCountBasedPlacementOptionsContributor : ConfigureOptionsContributorBase<ActivationCountBasedPlacementOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:ActivationCountBasedPlacement";
}
