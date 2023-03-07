using System.ComponentModel.DataAnnotations;
using Fluxera.ComponentModel.Annotations;
using Fluxera.Entity;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class SnackBase : AggregateRoot<SnackBase, long>, ISoftDeleteObject, IAuditedObject
{
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}
