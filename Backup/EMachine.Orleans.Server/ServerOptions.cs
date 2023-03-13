using System.Net;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
public sealed class ServerOptions
{
    public ClusterOptions Cluster { get; set; } = new();

    public EndpointOptions Endpoint { get; set; } = new();
    
    /// <summary>
    ///     The silo name.
    /// </summary>
    public string? SiloName { get; set; }
}
