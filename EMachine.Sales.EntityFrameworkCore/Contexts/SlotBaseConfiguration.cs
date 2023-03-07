using EMachine.Sales.Domain.Abstractions.Entities;
using Fluxera.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SlotBaseConfiguration : IEntityTypeConfiguration<SlotBase>
{
    private readonly Action<EntityTypeBuilder<SlotBase>>? _callback;

    public SlotBaseConfiguration(Action<EntityTypeBuilder<SlotBase>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SlotBase> builder)
    {
        builder.ToTable("Slots");
        builder.UseRepositoryDefaults();
        builder.HasKey(x => new
                            {
                                x.MachineId,
                                x.Position
                            });
        builder.HasOne<SnackMachineBase>().WithMany(x => x.Slots).HasForeignKey(x => x.MachineId);
        builder.HasOne<SnackBase>().WithMany().HasForeignKey(x => x.SnackId);
        _callback?.Invoke(builder);
    }
}
