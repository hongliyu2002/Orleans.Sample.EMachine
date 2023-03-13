using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
public sealed class ClusterOptions
{
    /// <summary>
    ///     A unique identifier for this service, which should survive deployment and redeployment, where as <see cref="ClusterId" /> might not.
    /// </summary>
    public string ServiceId { get; set; } = "default";

    /// <summary>
    ///     The cluster identity.
    /// </summary>
    public string ClusterId { get; set; } = "default";
}
