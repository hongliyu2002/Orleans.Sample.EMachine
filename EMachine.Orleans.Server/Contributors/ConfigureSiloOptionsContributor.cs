using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureSiloOptionsContributor : ConfigureOptionsContributorBase<SiloOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Silo";
}
