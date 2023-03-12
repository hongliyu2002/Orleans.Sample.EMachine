using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace EMachine.Persistence;

/// <summary>
///     A default database connection string provider that gets the name from configuration.
/// </summary>
[PublicAPI]
public class DefaultDatabaseConnectionStringProvider : IDatabaseConnectionStringProvider
{
    /// <summary>
    ///     Crates a new instance of the <see cref="DefaultDatabaseNameProvider" /> type.
    /// </summary>
    /// <param name="persistenceOptions"></param>
    public DefaultDatabaseConnectionStringProvider(IOptions<PersistenceOptions> persistenceOptions)
    {
        Options = persistenceOptions.Value;
    }

    /// <summary>
    ///     Gets the persistence options.
    /// </summary>
    protected PersistenceOptions Options { get; }

    /// <inheritdoc />
    public virtual string? GetConnectionString(string name)
    {
        // No tenant is needed to create the connection string.
        // Use the default connection string, if the aggregate belongs to it.
        if (Options.Databases.TryGetValue(name, out var databaseOptions))
        {
            // Get the connection string for the repository.
            return Options.ConnectionStrings.TryGetValue(databaseOptions.ConnectionStringName, out var connectionString) ? connectionString : null;
        }
        return null;
    }
}
