using System.Text.Json.Serialization;
using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace EMachine.Sales.Orleans.EntityFrameworkCore;

[PublicAPI]
public sealed class SalesDbOptions
{
    /// <summary>
    ///     Creates a new instance of the <see cref="SalesDbOptions" /> type.
    /// </summary>
    public SalesDbOptions()
    {
        ConnectionStrings = new ConnectionStrings();
    }

    /// <summary>
    ///     The name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "SalesDb";

    /// <summary>
    ///     The name and database provider type of the connection string.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SalesDbProvider DatabaseProvider { get; set; } = SalesDbProvider.SqlServer;

    /// <summary>
    /// </summary>
    public string? MigrationsHistoryTable { get; set; }

    /// <summary>
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuerySplittingBehavior QuerySplittingBehavior { get; set; } = QuerySplittingBehavior.SingleQuery;

    /// <summary>
    /// </summary>
    public int MaxRetry { get; set; } = 3;

    /// <summary>
    /// </summary>
    public int MaxRetryDelay { get; set; } = 1000;

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; }
}
