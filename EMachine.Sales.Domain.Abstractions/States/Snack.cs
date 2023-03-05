using EMachine.Sales.Domain.Abstractions.Events;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;

namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class Snack : ISoftDeleteObject, IAuditedObject
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public string Name { get; set; } = string.Empty;

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
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(6)]
    public string LastModifiedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(7)]
    public string DeletedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(8)]
    public bool IsDeleted { get; set; }

    #region Apply

    public void Apply(SnackInitializedEvent evt)
    {
        Id = evt.Id;
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

    #endregion

}
