using EMachine.Sales.Domain.Abstractions.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

[PublicAPI]
public static class ModelBuilderExtensions
{
    public static void AddSnackBase(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<SnackBase>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SnackBaseConfiguration(callback));
    }

    public static void AddSlotBase(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<SlotBase>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SlotBaseConfiguration(callback));
    }

    public static void AddSnackMachineBase(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<SnackMachineBase>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SnackMachineBaseConfiguration(callback));
    }
}
