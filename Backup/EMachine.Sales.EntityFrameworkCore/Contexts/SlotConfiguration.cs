using EMachine.Sales.Domain.Entities;
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
        builder.HasIndex(x => x.ID);
        builder.HasKey(x => new
                            {
                                x.MachineUuId,
                                x.Position
                            });
        builder.HasOne<SnackMachine>().WithMany(x => x.Slots).HasForeignKey(x => x.MachineUuId);
        builder.OwnsOne(x => x.SnackPile, nav =>
                                          {
                                              nav.HasOne<Snack>(sp => sp.Snack).WithMany().HasForeignKey(sp => sp.SnackUuId);
                                          });
        _callback?.Invoke(builder);
    }
}
