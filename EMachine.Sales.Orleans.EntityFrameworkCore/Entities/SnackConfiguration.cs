using EMachine.Sales.Orleans.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.Orleans.EntityFrameworkCore;

public sealed class SnackConfiguration : IEntityTypeConfiguration<Snack>
{
    private readonly Action<EntityTypeBuilder<Snack>>? _callback;

    public SnackConfiguration(Action<EntityTypeBuilder<Snack>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Snack> builder)
    {
        builder.ToTable("Snacks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedBy).HasMaxLength(100);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(100);
        builder.Property(x => x.DeletedBy).HasMaxLength(100);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.PictureUrl).HasMaxLength(500);
        // builder.Property(x => x.Version).IsConcurrencyToken();
        _callback?.Invoke(builder);
    }
}
