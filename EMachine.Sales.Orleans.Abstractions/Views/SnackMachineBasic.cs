using System.Collections.Immutable;
using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Views;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineBasic(Guid Id, Money MoneyInside, decimal AmountInTransaction, int SlotsCount, decimal TotalPrice, IImmutableList<Slot> Slots);
