﻿using EMachine.Sales.Domain.Entities;
using Fluxera.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackMachineEntityConfiguration : IEntityTypeConfiguration<SnackMachine>
{
    private readonly Action<EntityTypeBuilder<SnackMachine>>? _callback;

    public SnackMachineEntityConfiguration(Action<EntityTypeBuilder<SnackMachine>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SnackMachine> builder)
    {
        builder.ToTable("SnackMachines");
        builder.HasIndex(x => x.ID)
               .IsUnique();
        builder.HasKey(x => x.UuId);
        builder.OwnsOne<Money>(x => x.MoneyInside);
        builder.Property(x => x.CreatedBy)
               .HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy)
               .HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy)
               .HasMaxLength(100);
        builder.UseRepositoryDefaults();
        _callback?.Invoke(builder);
    }
}
