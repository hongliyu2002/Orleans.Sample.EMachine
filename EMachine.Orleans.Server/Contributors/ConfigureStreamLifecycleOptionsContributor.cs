using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureStreamLifecycleOptionsContributor : ConfigureOptionsContributorBase<StreamLifecycleOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:StreamLifecycle";
}
