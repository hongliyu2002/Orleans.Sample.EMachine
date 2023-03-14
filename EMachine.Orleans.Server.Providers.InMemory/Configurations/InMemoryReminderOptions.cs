using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.InMemory;

[PublicAPI]
public sealed class InMemoryReminderOptions
{
    /// <summary>
    ///     Indicates if the feature is enabled or not.
    /// </summary>
    public bool FeatureEnabled { get; set; }
}
