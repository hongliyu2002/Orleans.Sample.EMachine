using EMachine.Sales.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SlotEntityConfiguration : IEntityTypeConfiguration<Slot>
{
    private readonly Action<EntityTypeBuilder<Slot>>? _callback;

    public SlotEntityConfiguration(Action<EntityTypeBuilder<Slot>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("Slots");
        builder.HasKey(x => new
                            {
                                x.MachineKey,
                                x.Position
                            });
        builder.HasOne<SnackMachine>().WithMany(x => x.Slots).HasForeignKey(x => x.MachineKey).OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne<SnackPile>(x => x.SnackPile, nav =>
                                                     {
                                                         nav.HasOne<Snack>(x => x.Snack).WithMany().HasForeignKey(x => x.SnackKey).OnDelete(DeleteBehavior.Cascade);
                                                     });
        _callback?.Invoke(builder);
    }
}
