﻿using EMachine.Sales.Domain.Entities;
using Fluxera.Repository.EntityFrameworkCore;
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
        builder.HasIndex(x => new
                              {
                                  x.MachineUuId,
                                  x.Position
                              })
               .IsUnique();
        builder.HasOne<SnackMachine>().WithMany(x => x.Slots).HasForeignKey(x => x.MachineUuId);
        builder.HasOne<Snack>(x => x.Snack).WithMany().HasForeignKey(x => x.SnackUuId);
        builder.UseRepositoryDefaults();
        _callback?.Invoke(builder);
    }
}
