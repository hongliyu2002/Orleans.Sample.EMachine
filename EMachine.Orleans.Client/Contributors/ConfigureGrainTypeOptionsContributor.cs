using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Client.Contributors;

internal sealed class ConfigureGrainTypeOptionsContributor : ConfigureOptionsContributorBase<GrainTypeOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Client:GrainType";
}
