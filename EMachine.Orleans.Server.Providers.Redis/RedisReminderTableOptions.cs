using Fluxera.Extensions.DataManagement;

namespace EMachine.Orleans.Server.Providers.Redis;

public class RedisReminderTableOptions
{
    /// <summary>
    ///     Gets or sets the name of the connection string.
    /// </summary>
    public string ConnectionStringName { get; set; } = "RedisReminder";

    /// <summary>
    ///     Gets the connection strings.
    /// </summary>
    public ConnectionStrings ConnectionStrings { get; internal set; } = new();
}
