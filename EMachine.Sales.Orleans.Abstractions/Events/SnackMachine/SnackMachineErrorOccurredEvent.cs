using System.Collections.Immutable;
using EMachine.Orleans.Shared.Events;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineErrorOccurredEvent(Guid Id, int Code, IImmutableList<string> Reasons, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version)
    : ErrorOccurredEvent(Code, Reasons, TraceId, OperatedAt, OperatedBy, Version);
