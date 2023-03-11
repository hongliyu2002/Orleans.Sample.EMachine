using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureActivationCountBasedPlacementOptionsContributor : ConfigureOptionsContributorBase<ActivationCountBasedPlacementOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:ActivationCountBasedPlacement";
}
