using JetBrains.Annotations;

namespace EMachine.Persistence;

[PublicAPI]
public class DatabaseOptions
{
    /// <summary>
    ///     Gets the name of the database provider.
    /// </summary>
    public string ProviderName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets the name of the database.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets the database name prefix.
    /// </summary>
    public string? DatabaseNamePrefix { get; set; }

    /// <summary>
    ///     Gets the name of the connection string to use.
    /// </summary>
    public string ConnectionStringName { get; set; } = string.Empty;
}
