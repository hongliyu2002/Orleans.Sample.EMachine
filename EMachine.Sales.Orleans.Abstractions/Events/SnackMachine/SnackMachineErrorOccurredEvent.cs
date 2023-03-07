using EMachine.Orleans.Shared.Events;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackMachineErrorOccurredEvent(Guid id, int code, string message, Guid traceId, string operatedBy)
        : base(code, message, traceId, operatedBy)
    {
        Id = id;
    }

    public SnackMachineErrorOccurredEvent(Guid id, int code, string message, string causedBy, Guid traceId, string operatedBy)
        : base(code, message, causedBy, traceId, operatedBy)
    {
        Id = id;
    }

    [Id(0)]
    public Guid Id { get; }
}
