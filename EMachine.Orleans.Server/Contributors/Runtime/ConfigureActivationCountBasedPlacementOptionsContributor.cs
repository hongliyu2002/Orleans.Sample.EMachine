using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

/// <summary>
///     Settings which regulate the placement of grains across a cluster when using <see cref="T:Orleans.Runtime.ActivationCountBasedPlacement" />.
/// </summary>
internal sealed class ConfigureActivationCountBasedPlacementOptionsContributor : ConfigureOptionsContributorBase<ActivationCountBasedPlacementOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:Runtime:ActivationCountBasedPlacement";
}
