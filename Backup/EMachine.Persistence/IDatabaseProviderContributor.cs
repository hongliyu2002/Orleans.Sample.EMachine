using Fluxera.Extensions.Hosting;
using JetBrains.Annotations;

namespace EMachine.Persistence;

/// <summary>
///     A contract for a database provider contributor.
/// </summary>
[PublicAPI]
public interface IDatabaseProviderContributor
{
    /// <summary>
    ///     The name of the database provider.
    /// </summary>
    public string DatabaseProviderName { get; }

    /// <summary>
    ///     Gets an action that adds a database provided by this contributor.
    /// </summary>
    Action<string, IServiceConfigurationContext> AddDatabase { get; }
}
