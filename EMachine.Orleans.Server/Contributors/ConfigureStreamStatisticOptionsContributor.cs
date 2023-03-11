using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureStreamStatisticOptionsContributor : ConfigureOptionsContributorBase<StreamStatisticOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:StreamStatistic";
}
