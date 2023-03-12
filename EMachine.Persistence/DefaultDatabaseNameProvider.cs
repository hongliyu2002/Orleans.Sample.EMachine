using Fluxera.Utilities.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace EMachine.Persistence;

/// <summary>
///     A default database name provider that gets the name from configuration.
/// </summary>
[PublicAPI]
public class DefaultDatabaseNameProvider : IDatabaseNameProvider
{
    /// <summary>
    ///     Crates a new instance of the <see cref="DefaultDatabaseNameProvider" /> type.
    /// </summary>
    /// <param name="persistenceOptions"></param>
    public DefaultDatabaseNameProvider(IOptions<PersistenceOptions> persistenceOptions)
    {
        Options = persistenceOptions.Value;
    }

    /// <summary>
    ///     Gets the persistence options.
    /// </summary>
    protected PersistenceOptions Options { get; }

    /// <inheritdoc />
    public virtual string? GetDatabaseName(string name)
    {
        // No tenant is needed to create the database name.
        // Use the default database name, if the aggregate belongs to it.
        if (Options.Databases.TryGetValue(name, out var databaseOptions))
        {
            return databaseOptions.DatabaseNamePrefix.IsNullOrWhiteSpace() ? databaseOptions.DatabaseName : $"{databaseOptions.DatabaseNamePrefix}-{databaseOptions.DatabaseName}";
        }
        return null;
    }
}
