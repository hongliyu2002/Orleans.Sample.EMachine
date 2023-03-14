using EMachine.Sales.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackMachineConfiguration : IEntityTypeConfiguration<SnackMachine>
{
    private readonly Action<EntityTypeBuilder<SnackMachine>>? _callback;

    public SnackMachineConfiguration(Action<EntityTypeBuilder<SnackMachine>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SnackMachine> builder)
    {
        builder.ToTable("SnackMachines");
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.MoneyInside, nav =>
                                            {
                                                nav.Property(n => n.Amount).HasPrecision(10, 2);
                                            });
        builder.Property(x => x.AmountInTransaction).HasPrecision(10, 2);
        builder.Property(x => x.TotalPrice).HasPrecision(10, 2);
        builder.Property(x => x.CreatedBy).HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(100);
        // builder.Property(x => x.Version).IsConcurrencyToken();
        _callback?.Invoke(builder);
    }
}
