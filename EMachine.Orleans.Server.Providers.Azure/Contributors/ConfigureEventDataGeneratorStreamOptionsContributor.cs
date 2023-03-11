using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.Azure.Contributors;

internal sealed class ConfigureEventDataGeneratorStreamOptionsContributor : ConfigureOptionsContributorBase<EventDataGeneratorStreamOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:EventDataGeneratorStream";
}
