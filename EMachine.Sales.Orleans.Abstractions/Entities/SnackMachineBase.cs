using Fluxera.Entity;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;
using JetBrains.Annotations;

namespace EMachine.Sales.Orleans.Abstractions.Entities;

[PublicAPI]
public sealed class SnackMachineBase : AggregateRoot<SnackMachineBase, Guid>, ISoftDeleteObject, IAuditedObject
{
    public int InsideYuan1 { get; }

    public int InsideYuan2 { get; }

    public int InsideYuan5 { get; }

    public int InsideYuan10 { get; }

    public int InsideYuan20 { get; }

    public int InsideYuan50 { get; }

    public int InsideYuan100 { get; }

    public decimal AmountInTransaction { get; set; }

    public IList<SlotBase> Slots { get; set; } = new List<SlotBase>();

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}
