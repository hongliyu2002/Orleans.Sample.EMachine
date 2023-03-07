using Fluxera.Entity;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class SnackMachineBase : AggregateRoot<SnackMachineBase, Guid>, ISoftDeleteObject, IAuditedObject
{
    public int InsideYuan1 { get; set; }

    public int InsideYuan2 { get; set; }

    public int InsideYuan5 { get; set; }

    public int InsideYuan10 { get; set; }

    public int InsideYuan20 { get; set; }

    public int InsideYuan50 { get; set; }

    public int InsideYuan100 { get; set; }

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
