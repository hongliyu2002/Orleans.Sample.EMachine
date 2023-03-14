using EMachine.Sales.Domain;
using Microsoft.EntityFrameworkCore;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SalesDbContext : DbContext
{
    public SalesDbContext()
    {
    }

    public SalesDbContext(DbContextOptions<SalesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Snack> Snacks { get; set; } = null!;

    public DbSet<SnackId> SnackIds { get; set; } = null!;

    public DbSet<Slot> Slots { get; set; } = null!;

    public DbSet<SnackMachine> SnackMachines { get; set; } = null!;

    public DbSet<SnackMachineId> SnackMachineIds { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddSnack();
        modelBuilder.AddSnackId();
        modelBuilder.AddSlot();
        modelBuilder.AddSnackMachine();
        modelBuilder.AddSnackMachineId();
    }
}
