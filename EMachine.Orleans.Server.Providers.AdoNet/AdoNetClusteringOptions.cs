using System.Text.Json.Serialization;
using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;
using Machine.Orleans.Server.Providers.AdoNet;

namespace EMachine.Orleans.Server.Providers.AdoNet;

[PublicAPI]
public sealed class AdoNetClusteringOptions
{
    /// <summary>
    ///     Gets or sets the name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "AdoNetCluster";

    /// <summary>
    ///     Gets or sets the name and database provider type of the connection string.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdoNetDatabaseProvider DatabaseProvider { get; set; } = AdoNetDatabaseProvider.SqlServer;

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
