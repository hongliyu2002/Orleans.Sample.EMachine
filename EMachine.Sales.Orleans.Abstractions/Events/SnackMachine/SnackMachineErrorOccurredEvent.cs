using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackMachineErrorOccurredEvent(Guid key, int code, string message, Guid traceId, string operatedBy)
        : base(code, message, traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
    }

    public SnackMachineErrorOccurredEvent(Guid key, int code, string message, string causedBy, Guid traceId, string operatedBy)
        : base(code, message, causedBy, traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
    }

    [Id(0)]
    public Guid Key { get; }
}
