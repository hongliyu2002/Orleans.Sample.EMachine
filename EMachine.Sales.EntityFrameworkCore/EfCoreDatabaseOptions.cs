using System.Text.Json.Serialization;
using Fluxera.Extensions.DataManagement;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace EMachine.Sales.EntityFrameworkCore;

[PublicAPI]
public sealed class EfCoreDatabaseOptions
{
    /// <summary>
    ///     Gets or sets the name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "SalesDatabase";

    /// <summary>
    ///     Gets or sets the name and database provider type of the connection string.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EfDatabaseProvider DatabaseProvider { get; set; } = EfDatabaseProvider.SqlServer;

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
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
