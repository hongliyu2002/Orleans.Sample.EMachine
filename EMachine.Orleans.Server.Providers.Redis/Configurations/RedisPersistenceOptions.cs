using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.Redis;

[PublicAPI]
public sealed class RedisPersistenceOptions
{
    /// <summary>
    ///     The name of the connection string.
    /// </summary>
    public string[] ConnectionStringNames { get; set; } = Array.Empty<string>();

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
