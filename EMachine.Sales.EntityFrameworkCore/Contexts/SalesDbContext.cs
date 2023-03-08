using Fluxera.Extensions.Hosting.Modules.Persistence;
using Fluxera.Repository;
using Microsoft.EntityFrameworkCore;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SalesDbContext : DbContext
{
    private readonly IDatabaseConnectionStringProvider? _databaseConnectionStringProvider;
    private readonly IDatabaseNameProvider? _databaseNameProvider;

    public SalesDbContext()
    {
    }

    public SalesDbContext(DbContextOptions<SalesDbContext> options, IDatabaseNameProvider? databaseNameProvider = null, IDatabaseConnectionStringProvider? databaseConnectionStringProvider = null)
        : base(options)
    {
        _databaseNameProvider = databaseNameProvider;
        _databaseConnectionStringProvider = databaseConnectionStringProvider;

        // Database.EnsureDeleted();
        // Database.EnsureCreated();
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var repositoryName = new RepositoryName("Sales");
            var connectionString = _databaseConnectionStringProvider?.GetConnectionString(repositoryName);
            connectionString ??= "Data Source=Sales.db";
            optionsBuilder.UseSqlite(connectionString);

            // var databaseName = _databaseNameProvider?.GetDatabaseName(repositoryName);
            // var connectionString = _databaseConnectionStringProvider?.GetConnectionString(repositoryName);
            // connectionString ??= "Server=localhost;Integrated Security=False;User Id=sa;Password=Bosshong2010;TrustServerCertificate=True;";
            // connectionString = connectionString.EnsureEndsWith(";");
            // connectionString += $"Database={databaseName ?? "Sales"}";
            // optionsBuilder.UseSqlServer(connectionString);
        }
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add the domain entities.
        modelBuilder.AddSnackEntity();
        modelBuilder.AddSlotEntity();
        modelBuilder.AddSnackMachineEntity();
    }
}
