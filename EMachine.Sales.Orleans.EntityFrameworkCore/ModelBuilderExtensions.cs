using EMachine.Sales.Orleans.States;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.Orleans.EntityFrameworkCore;

[PublicAPI]
public static class ModelBuilderExtensions
{
    public static void AddSnack(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<Snack>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SnackConfiguration(callback));
    }

    public static void AddSlot(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<Slot>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SlotConfiguration(callback));
    }

    public static void AddSnackMachine(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<SnackMachine>>? callback = null)
    {
        modelBuilder.ApplyConfiguration(new SnackMachineConfiguration(callback));
    }
}
