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
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineRemovedEvent evt)
    {
        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = evt.OperatedBy;
        IsDeleted = true;
    }

    public void Apply(SnackMachineLoadedMoneyEvent evt)
    {
        MoneyInside += evt.Money;
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineUnloadedMoneyEvent evt)
    {
        MoneyInside = Money.Zero;
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineInsertedMoneyEvent evt)
    {
        AmountInTransaction += evt.Money.Amount;
        MoneyInside += evt.Money;
        LastModifiedAt = DateTimeOffset.UtcNow;
        LastModifiedBy = evt.OperatedBy;
    }

    public void Apply(SnackMachineReturnedMoneyEvent evt)
    {
        if (MoneyInside.TryAllocate(AmountInTransaction, out var moneyAllocated))
        {
            MoneyInside -= moneyAllocated;
            LastModifiedAt = DateTimeOffset.UtcNow;
            LastModifiedBy = evt.OperatedBy;
        }
    }

    public void Apply(SnackMachineLoadedSnacksEvent evt)
    {
        if (TryGetSlot(evt.Position, out var slot) && slot is { SnackPile: { } })
        {
            slot.SnackPile = evt.SnackPile;
            LastModifiedAt = DateTimeOffset.UtcNow;
            LastModifiedBy = evt.OperatedBy;
        }
    }

    public void Apply(SnackMachineBoughtSnackEvent evt)
    {
        if (TryGetSlot(evt.Position, out var slot) && slot is { SnackPile: { } } && slot.SnackPile.TryPopOne(out var snackPilePopped) && snackPilePopped is { })
        {
            slot.SnackPile = snackPilePopped;
            AmountInTransaction -= snackPilePopped.Price;
            LastModifiedAt = DateTimeOffset.UtcNow;
            LastModifiedBy = evt.OperatedBy;
        }
    }

    #endregion

}
