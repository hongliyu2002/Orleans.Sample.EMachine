using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureServerOptionsContributor : ConfigureOptionsContributorBase<ServerOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server";
}
