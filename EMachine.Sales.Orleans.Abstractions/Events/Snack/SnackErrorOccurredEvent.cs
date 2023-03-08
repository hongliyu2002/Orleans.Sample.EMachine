using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackErrorOccurredEvent(Guid uuId, int code, string message, Guid traceId, string operatedBy)
        : base(code, message, traceId, operatedBy)
    {
        UuId = Guard.Against.Empty(uuId, nameof(uuId));
    }

    public SnackErrorOccurredEvent(Guid uuId, int code, string message, string causedBy, Guid traceId, string operatedBy)
        : base(code, message, causedBy, traceId, operatedBy)
    {
        UuId = Guard.Against.Empty(uuId, nameof(uuId));
    }

    [Id(0)]
    public Guid UuId { get; }
}
