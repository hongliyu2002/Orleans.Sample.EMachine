using EMachine.Sales.Domain.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

[PublicAPI]
public static class ModelBuilderExtensions
{
    public static void AddSnackEntity(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<Snack>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SnackEntityConfiguration(callback));
    }

    public static void AddSlotEntity(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<Slot>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SlotEntityConfiguration(callback));
    }

    public static void AddSnackMachineEntity(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<SnackMachine>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SnackMachineEntityConfiguration(callback));
    }
}
