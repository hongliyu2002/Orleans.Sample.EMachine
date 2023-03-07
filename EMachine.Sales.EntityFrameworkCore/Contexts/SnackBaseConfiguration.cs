﻿using EMachine.Sales.Domain.Entities;
using Fluxera.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackBaseConfiguration : IEntityTypeConfiguration<SnackBase>
{
    private readonly Action<EntityTypeBuilder<SnackBase>>? _callback;

    public SnackBaseConfiguration(Action<EntityTypeBuilder<SnackBase>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SnackBase> builder)
    {
        builder.ToTable("Snacks");
        builder.HasKey(x => x.ID);
        builder.Property(x => x.ID).ValueGeneratedNever();
        builder.UseRepositoryDefaults();
        _callback?.Invoke(builder);
    }
}
