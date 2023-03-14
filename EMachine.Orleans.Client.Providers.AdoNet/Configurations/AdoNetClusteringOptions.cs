using System.Text.Json.Serialization;
using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Orleans.Client.Providers.AdoNet;

[PublicAPI]
public sealed class AdoNetClusteringOptions
{
    /// <summary>
    ///     The name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "AdoNetCluster";

    /// <summary>
    ///     The name and database provider type of the connection string.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdoNetDatabaseProvider DatabaseProvider { get; set; } = AdoNetDatabaseProvider.SqlServer;

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
