using System.Net;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
public sealed class ServerEndpointOptions
{
    /// <summary>
    ///     The IP address used for clustering.
    /// </summary>
    public IPAddress ClusterIPAddress { get; set; } = IPAddress.Loopback;

    /// <summary>
    ///     The port this silo uses for silo-to-silo communication.
    /// </summary>
    public int SiloPort { get; set; } = 11111;

    /// <summary>
    ///     The endpoint used to listen for silo to silo communication.
    ///     If not set will default to <see cref="ClusterIPAddress" /> + <see cref="SiloPort" />
    /// </summary>
    public IPEndPoint? SiloListeningEndpoint { get; set; }

    /// <summary>
    ///     The port this silo uses for silo-to-client (gateway) communication. Specify 0 to disable gateway functionality.
    /// </summary>
    public int GatewayPort { get; set; } = 30000;

    /// <summary>
    ///     The endpoint used to listen for client to silo communication.
    ///     If not set will default to <see cref="ClusterIPAddress" /> + <see cref="GatewayPort" />
    /// </summary>
    public IPEndPoint? GatewayListeningEndpoint { get; set; }
}
