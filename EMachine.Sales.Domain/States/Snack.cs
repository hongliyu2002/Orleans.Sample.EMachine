using EMachine.Sales.Domain.Abstractions.Events;
using Fluxera.Entity;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;
using Fluxera.Guards;

namespace EMachine.Sales.Domain;

[GenerateSerializer]
public sealed class Snack : Entity<Snack, Guid>, ISoftDeleteObject, IAuditedObject
{
    /// <inheritdoc />
    public Snack()
    {
        Name = string.Empty;
        CreatedBy = string.Empty;
        LastModifiedBy = string.Empty;
        DeletedBy = string.Empty;
    }

    /// <inheritdoc />
    public Snack(Guid id, string name)
        : this()
    {
        ID = Guard.Against.Empty(id, nameof(id));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    /// <inheritdoc />
    [Id(0)]
    public override Guid ID { get; set; }

    [Id(1)]
    public string Name { get; set; }

    /// <inheritdoc />
    [Id(2)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    [Id(3)]
    public DateTimeOffset? LastModifiedAt { get; set; }

    /// <inheritdoc />
    [Id(4)]
    public DateTimeOffset? DeletedAt { get; set; }

    /// <inheritdoc />
    [Id(5)]
    public string CreatedBy { get; set; }

    /// <inheritdoc />
    [Id(6)]
    public string LastModifiedBy { get; set; }

    /// <inheritdoc />
    [Id(7)]
    public string DeletedBy { get; set; }

    /// <inheritdoc />
    [Id(8)]
    public bool IsDeleted { get; set; }

    public void Apply(SnackInitializedEvent evt)
    {
        ID = evt.Id;
        Name = evt.Name;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = evt.OperatedBy;
    }

    public void Apply(SnackNameChangedEvent evt)
    {
        Name = evt.Name;
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackRemovedEvent evt)
    {
        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = evt.OperatedBy;
        IsDeleted = true;
    }
}
