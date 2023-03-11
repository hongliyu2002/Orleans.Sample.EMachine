using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureSiloMessagingOptionsContributor : ConfigureOptionsContributorBase<SiloMessagingOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:SiloMessaging";
}
