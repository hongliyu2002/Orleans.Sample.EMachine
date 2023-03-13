using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
public sealed class ServerOptions
{
    /// <summary>
    ///     Gets or sets the name of the broadcast channel.
    /// </summary>
    public string[] BroadcastChannelNames { get; set; } = Array.Empty<string>();
}
