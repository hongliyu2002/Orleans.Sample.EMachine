using System.Net;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
public sealed class ServerOptions
{
    public ServerClusterOptions Cluster { get; set; } = new();

    public ServerEndpointOptions Endpoint { get; set; } = new();

    public ServerSiloOptions? Silo { get; set; }
}
