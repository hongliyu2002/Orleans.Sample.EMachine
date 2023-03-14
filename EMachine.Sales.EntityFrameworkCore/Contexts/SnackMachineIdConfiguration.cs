using EMachine.Sales.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackMachineIdConfiguration : IEntityTypeConfiguration<SnackMachineId>
{
    private readonly Action<EntityTypeBuilder<SnackMachineId>>? _callback;

    public SnackMachineIdConfiguration(Action<EntityTypeBuilder<SnackMachineId>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SnackMachineId> builder)
    {
        builder.ToTable("SnackMachineIds");
        builder.HasKey(x => x.Id);
        _callback?.Invoke(builder);
    }
}
