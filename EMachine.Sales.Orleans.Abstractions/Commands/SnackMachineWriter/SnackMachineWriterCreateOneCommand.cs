﻿using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Sales.Orleans.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterCreateOneCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterCreateOneCommand(Guid uuId, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        UuId = Guard.Against.Empty(uuId, nameof(uuId));
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Guid UuId { get; }

    [Id(1)]
    public Money MoneyInside { get; }

    [Id(2)]
    public IImmutableList<Slot> Slots { get; }
}
