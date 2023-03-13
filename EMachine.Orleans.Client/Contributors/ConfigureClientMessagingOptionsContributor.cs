using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Client.Contributors;

internal sealed class ConfigureClientMessagingOptionsContributor : ConfigureOptionsContributorBase<ClientMessagingOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Client:ClientMessaging";
}
