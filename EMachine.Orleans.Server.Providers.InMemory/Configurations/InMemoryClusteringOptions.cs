using System.Net;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.InMemory;

[PublicAPI]
public sealed class InMemoryClusteringOptions
{
    /// <summary>
    ///     Indicates if the feature is enabled or not.
    /// </summary>
    public bool FeatureEnabled { get; set; }

    /// <summary>
    ///     The seed node to find the membership system grain.
    /// </summary>
    public IPEndPoint? PrimarySiloEndpoint { get; set; }
}
