﻿using EMachine.Domain.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackErrorOccurredEvent(long id, int code, string message, Guid traceId, string operatedBy)
        : base(code, message, traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
    }

    public SnackErrorOccurredEvent(long id, int code, string message, string causedBy, Guid traceId, string operatedBy)
        : base(code, message, causedBy, traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
    }

    [Id(0)]
    public long Id { get; }
}