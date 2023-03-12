using JetBrains.Annotations;

namespace EMachine.Persistence;

/// <summary>
///     A class containing the supported database providers.
/// </summary>
[PublicAPI]
public static class DatabaseProviderNames
{
    /// <summary>
    ///     The in-memory database provider.
    /// </summary>
    public const string InMemory = "InMemory";

    /// <summary>
    ///     The EFCore database provider.
    /// </summary>
    public const string EntityFrameworkCore = "EntityFrameworkCore";

    /// <summary>
    ///     The LiteDB database provider.
    /// </summary>
    public const string LiteDB = "LiteDB";

    /// <summary>
    ///     The MongoDB database provider.
    /// </summary>
    public const string MongoDB = "MongoDB";
}
