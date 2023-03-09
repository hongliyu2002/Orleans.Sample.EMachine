using System.Collections.Immutable;
using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializedEvent(Guid Id, Money MoneyInside, IImmutableList<Slot> Slots, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version)
    : SnackMachineEvent(Id, TraceId, OperatedAt, OperatedBy, Version);
