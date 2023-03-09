using EMachine.Sales.Orleans.Events;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;

namespace EMachine.Sales.Orleans.States;

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
        return $"SnackMachine with Id:'{Id}' MoneyInside:'{MoneyInside}' AmountInTransaction:{AmountInTransaction} Slots:'{string.Join(';', Slots.Select(slot => slot.ToString()))}'";
    }

    #region Try Get

    public bool TryGetSlot(int position, out Slot? slot)
    {
        slot = Slots.FirstOrDefault(x => x.Position == position);
        return slot != null;
    }

    #endregion

    #region Apply

    public void Apply(SnackMachineInitializedEvent evt)
    {
        Id = evt.Id;
        MoneyInside = evt.MoneyInside;
        Slots = evt.Slots.ToList();
        CreatedAt = evt.OperatedAt;
        CreatedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineRemovedEvent evt)
    {
        DeletedAt = evt.OperatedAt;
        DeletedBy = evt.OperatedBy;
        IsDeleted = true;
    }

    public void Apply(SnackMachineMoneyLoadedEvent evt)
    {
        MoneyInside += evt.Money;
        LastModifiedAt = evt.OperatedAt;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineMoneyUnloadedEvent evt)
    {
        MoneyInside = Money.Zero;
        LastModifiedAt = evt.OperatedAt;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineMoneyInsertedEvent evt)
    {
        AmountInTransaction += evt.Money.Amount;
        MoneyInside += evt.Money;
        LastModifiedAt = evt.OperatedAt;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineMoneyReturnedEvent evt)
    {
        if (MoneyInside.TryAllocate(AmountInTransaction, out var moneyAllocated))
        {
            MoneyInside -= moneyAllocated;
            LastModifiedAt = evt.OperatedAt;
            LastModifiedBy = evt.OperatedBy;
        }
    }

    public void Apply(SnackMachineSnacksLoadedEvent evt)
    {
        if (TryGetSlot(evt.Position, out var slot) && slot is { SnackPile: { } })
        {
            slot.SnackPile = evt.SnackPile;
            LastModifiedAt = evt.OperatedAt;
            LastModifiedBy = evt.OperatedBy;
        }
    }

    public void Apply(SnackMachineSnackBoughtEvent evt)
    {
        if (TryGetSlot(evt.Position, out var slot) && slot is { SnackPile: { } } && slot.SnackPile.TryPopOne(out var snackPilePopped) && snackPilePopped is { })
        {
            slot.SnackPile = snackPilePopped;
            AmountInTransaction -= snackPilePopped.Price;
            LastModifiedAt = evt.OperatedAt;
            LastModifiedBy = evt.OperatedBy;
        }
    }

    #endregion

}
