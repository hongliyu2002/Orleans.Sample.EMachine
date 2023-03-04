using EMachine.Domain.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]

[GenerateSerializer]
public sealed class SnackErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackErrorOccurredEvent()
    {
    }

    public SnackErrorOccurredEvent(Guid id, int code, string message, Guid traceId, string operatedBy)
        : base(code, message, traceId, operatedBy)
    {
        ID = Guard.Against.Empty(id, nameof(id));
    }

    public SnackErrorOccurredEvent(Guid id, int code, string message, string causedBy, Guid traceId, string operatedBy)
        : base(code, message, causedBy, traceId, operatedBy)
    {
        ID = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid ID { get; set; }
}
