using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.Redis;

[PublicAPI]
public sealed class RedisClusteringOptions
{
    /// <summary>
    ///     Indicates if the feature is enabled or not.
    /// </summary>
    public bool FeatureEnabled { get; set; }

    /// <summary>
    ///     The name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "RedisCluster";

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
