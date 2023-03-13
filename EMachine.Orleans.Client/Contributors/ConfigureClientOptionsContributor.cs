using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Orleans.Client.Contributors;

internal sealed class ConfigureClientOptionsContributor : ConfigureOptionsContributorBase<ClientOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Client";
}
