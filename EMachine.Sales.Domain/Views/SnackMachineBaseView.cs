using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackMachineBaseView
{
    public Guid Id { get; set; }

    public Money MoneyInside { get; set; } = null!;

    public decimal AmountInTransaction { get; set; }

    public IList<Slot> Slots { get; set; } = new List<Slot>();

    public int SlotsCount { get; set; }

    public decimal TotalPrice { get; set; }

    #region Create

    public static SnackMachineBaseView Create(Guid id, Money moneyInside, decimal amountInTransaction, IList<Slot> slots, int slotsCount, decimal totalPrice)
    {
        return new SnackMachineBaseView
               {
                   Id = id,
                   MoneyInside = moneyInside,
                   AmountInTransaction = amountInTransaction,
                   Slots = slots,
                   SlotsCount = slotsCount,
                   TotalPrice = totalPrice
               };
    }

    #endregion

}
