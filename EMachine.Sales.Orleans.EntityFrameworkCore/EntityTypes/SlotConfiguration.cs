using EMachine.Sales.Orleans.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.Orleans.EntityFrameworkCore;

public sealed class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    private readonly Action<EntityTypeBuilder<Slot>>? _callback;

    public SlotConfiguration(Action<EntityTypeBuilder<Slot>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("Slots");
        builder.HasKey(x => new
                            {
                                x.MachineId,
                                x.Position
                            });
        builder.HasOne<SnackMachine>().WithMany(x => x.Slots).HasForeignKey(x => x.MachineId).OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne<SnackPile>(x => x.SnackPile, nav =>
                                                     {
                                                         nav.HasOne<Snack>().WithMany().HasForeignKey(x => x.SnackId).OnDelete(DeleteBehavior.Cascade);
                                                         nav.Property(n => n.Price).HasPrecision(10, 2);
                                                     });
        _callback?.Invoke(builder);
    }
}
