using System.Text.Json.Serialization;
using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.AdoNet;

[PublicAPI]
public sealed class AdoNetReminderOptions
{
    /// <summary>
    ///     Indicates if the feature is enabled or not.
    /// </summary>
    public bool FeatureEnabled { get; set; }

    /// <summary>
    ///     The name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "AdoNetReminder";

    /// <summary>
    ///     The name and database provider type of the connection string.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdoNetDbProvider DbProvider { get; set; } = AdoNetDbProvider.SqlServer;

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
