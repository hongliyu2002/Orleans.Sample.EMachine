﻿using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Sales.Orleans.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterCreateOneCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterCreateOneCommand(Guid key, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Guid Key { get; }

    [Id(1)]
    public Money MoneyInside { get; }

    [Id(2)]
    public IImmutableList<Slot> Slots { get; }
}
