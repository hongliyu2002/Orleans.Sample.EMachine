using JetBrains.Annotations;

namespace EMachine.Orleans.Client;

[PublicAPI]
public sealed class ClientOptions
{
    /// <summary>
    ///     The name of the broadcast channel.
    /// </summary>
    public string[] BroadcastChannelNames { get; set; } = Array.Empty<string>();
}
