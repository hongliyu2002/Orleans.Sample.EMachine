using JetBrains.Annotations;

namespace EMachine.Persistence;

/// <summary>
///     A contract for a database connection string provider.
/// </summary>
[PublicAPI]
public interface IDatabaseConnectionStringProvider
{
    /// <summary>
    ///     Gets the database connection string for the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string? GetConnectionString(string name);
}
