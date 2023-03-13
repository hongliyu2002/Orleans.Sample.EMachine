using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureGrainCollectionOptionsContributor : ConfigureOptionsContributorBase<GrainCollectionOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:GrainCollection";
}
