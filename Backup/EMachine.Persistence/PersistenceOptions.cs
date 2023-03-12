using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Persistence;

[PublicAPI]
public sealed class PersistenceOptions
{
    /// <summary>
    ///     Gets the options of the databases.
    /// </summary>
    public DatabaseOptionsDictionary Databases { get; set; } = new();

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; set; } = new();
}
