using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
public sealed class ServerSiloOptions
{
    /// <summary>
    ///     The silo name.
    /// </summary>
    public string? SiloName { get; set; }
}
