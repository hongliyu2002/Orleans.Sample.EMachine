using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Providers.Azure.Contributors;

internal sealed class ConfigureEventHubStreamCachePressureOptionsContributor : ConfigureOptionsContributorBase<EventHubStreamCachePressureOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:EventHubStreamCachePressure";
}
