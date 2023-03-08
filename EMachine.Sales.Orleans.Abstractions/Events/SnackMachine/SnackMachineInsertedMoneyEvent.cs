﻿using EMachine.Orleans.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInsertedMoneyEvent : SnackMachineEvent
{
    public SnackMachineInsertedMoneyEvent(Guid key, Money money, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
        Money = Guard.Against.Null(money);
    }

    [Id(0)]
    public Money Money { get; }
}
