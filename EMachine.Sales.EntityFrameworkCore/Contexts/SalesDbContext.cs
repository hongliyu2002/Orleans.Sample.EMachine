using Fluxera.Extensions.Hosting.Modules.Persistence;
using Fluxera.Repository;
using Fluxera.Utilities.Extensions;
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
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var repositoryName = new RepositoryName("Default");
            var databaseName = _databaseNameProvider?.GetDatabaseName(repositoryName);
            var connectionString = _databaseConnectionStringProvider?.GetConnectionString(repositoryName);
            connectionString ??= "Server=localhost;Integrated Security=True;TrustServerCertificate=True;";
            connectionString = connectionString.EnsureEndsWith(";");
            connectionString += $"Database={databaseName ?? "Sales"}";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add the domain entities.
        modelBuilder.AddSnackBase();
        modelBuilder.AddSlotBase();
        modelBuilder.AddSnackMachineBase();
    }
}
