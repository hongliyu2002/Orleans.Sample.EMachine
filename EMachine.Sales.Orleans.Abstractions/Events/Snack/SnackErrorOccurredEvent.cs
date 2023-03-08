using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackErrorOccurredEvent(Guid id, int code, string message, Guid traceId, string operatedBy)
        : base(code, message, traceId, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    public SnackErrorOccurredEvent(Guid id, int code, string message, string causedBy, Guid traceId, string operatedBy)
        : base(code, message, causedBy, traceId, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid Id { get; }
}
