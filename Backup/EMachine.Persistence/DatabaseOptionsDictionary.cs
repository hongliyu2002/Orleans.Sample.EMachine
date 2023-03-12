using Fluxera.Utilities.Extensions;
using JetBrains.Annotations;

namespace EMachine.Persistence;

[PublicAPI]
public sealed class DatabaseOptionsDictionary : Dictionary<string, DatabaseOptions>
{
    /// <summary>
    ///     The default database name.
    /// </summary>
    public const string DefaultDatabaseName = "Default";

    /// <summary>
    ///     Gets the default database options.
    /// </summary>
    public DatabaseOptions Default
    {
        get => this.GetOrDefault(DefaultDatabaseName);
        set => this[DefaultDatabaseName] = value;
    }

    /// <summary>
    ///     Gets the options for the given name, oder the default options if available.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public DatabaseOptions GetOptionsOrDefault(string name)
    {
        return this.GetOrDefault(name) ?? Default ?? throw new Exception($"The database '{name}' was not found and there are no default options.");
    }
}
