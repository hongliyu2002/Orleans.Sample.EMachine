using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class Snack
{
    public Guid Key { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}
