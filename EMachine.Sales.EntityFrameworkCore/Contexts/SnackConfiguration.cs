using EMachine.Sales.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackEntityConfiguration : IEntityTypeConfiguration<Snack>
{
    private readonly Action<EntityTypeBuilder<Snack>>? _callback;

    public SnackEntityConfiguration(Action<EntityTypeBuilder<Snack>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Snack> builder)
    {
        builder.ToTable("Snacks");
        builder.HasKey(x => x.Key);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.CreatedBy).HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(100);
        _callback?.Invoke(builder);
    }
}
