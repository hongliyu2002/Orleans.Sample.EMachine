using JetBrains.Annotations;

namespace EMachine.Orleans.Client;

[PublicAPI]
public sealed class ClientOptions
{
    /// <summary>
    ///     Gets or sets the name of the broadcast channel.
    /// </summary>
    public string[] BroadcastChannelNames { get; set; } = Array.Empty<string>();
}
