using EMachine.Domain.Shared;
using EMachine.Sales.Domain.Abstractions.Events;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;

namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class SnackMachine : ISoftDeleteObject, IAuditedObject
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public Money MoneyInside { get; set; } = Money.Zero;

    [Id(2)]
    public decimal AmountInTransaction { get; set; }

    [Id(3)]
    public IList<Slot> Slots { get; set; } = new List<Slot>();

    public bool IsCreated => CreatedAt != null;

    /// <inheritdoc />
    [Id(4)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    [Id(5)]
    public DateTimeOffset? LastModifiedAt { get; set; }

    /// <inheritdoc />
    [Id(6)]
    public DateTimeOffset? DeletedAt { get; set; }

    /// <inheritdoc />
    [Id(7)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(8)]
    public string LastModifiedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(9)]
    public string DeletedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(10)]
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"SnackMachine with Id:'{Id}' MoneyInside:'{MoneyInside}' AmountInTransaction:{AmountInTransaction} Slots:'{string.Join(';', Slots.Select(s => s.ToString()))}'";
    }

    #region Apply

    public void Apply(SnackMachineInitializedEvent evt)
    {
        Id = evt.Id;
        MoneyInside = evt.MoneyInside;
        Slots = evt.Slots.ToList();
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineRemovedEvent evt)
    {
        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = evt.OperatedBy;
        IsDeleted = true;
    }

    public void Apply(SnackMachineInsertedMoneyEvent evt)
    {
        AmountInTransaction += evt.Money.Amount;
        MoneyInside += evt.Money;
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = evt.OperatedBy;
    }

    #endregion

}
