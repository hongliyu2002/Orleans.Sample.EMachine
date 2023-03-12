using JetBrains.Annotations;

namespace EMachine.Persistence;

/// <summary>
///     A contract for a database name provider.
/// </summary>
[PublicAPI]
public interface IDatabaseNameProvider
{
    /// <summary>
    ///     Gets the database name for the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string? GetDatabaseName(string name);
}
