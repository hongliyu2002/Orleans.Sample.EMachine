using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using OpenTelemetry.Trace;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class TracerProviderContributor : ITracerProviderContributor
{

    /// <inheritdoc />
    public void Configure(TracerProviderBuilder builder, IServiceConfigurationContext context)
    {
        builder.AddRedisInstrumentation();
    }
}
