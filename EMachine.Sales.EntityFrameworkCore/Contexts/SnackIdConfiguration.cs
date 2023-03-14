using EMachine.Sales.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public sealed class SnackIdConfiguration : IEntityTypeConfiguration<SnackId>
{
    private readonly Action<EntityTypeBuilder<SnackId>>? _callback;

    public SnackIdConfiguration(Action<EntityTypeBuilder<SnackId>>? callback = null)
    {
        _callback = callback;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SnackId> builder)
    {
        builder.ToTable("SnackIds");
        builder.HasKey(x => x.Id);
        _callback?.Invoke(builder);
    }
}
