using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureSchedulingOptionsContributor : ConfigureOptionsContributorBase<SchedulingOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:Scheduling";
}
