using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureSchedulingOptionsContributor : ConfigureOptionsContributorBase<SchedulingOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Scheduling";
}
