﻿using EMachine.Sales.Orleans.Abstractions.Entities;
using Fluxera.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackMachineBaseConfiguration : IEntityTypeConfiguration<SnackMachineBase>
{
    private readonly Action<EntityTypeBuilder<SnackMachineBase>>? _callback;

    public SnackMachineBaseConfiguration(Action<EntityTypeBuilder<SnackMachineBase>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SnackMachineBase> builder)
    {
        builder.ToTable("SnackMachines");
        builder.UseRepositoryDefaults();
        _callback?.Invoke(builder);
    }
}
