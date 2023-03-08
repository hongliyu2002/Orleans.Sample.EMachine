using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineErrorOccurredEvent : ErrorOccurredEvent
{
    public SnackMachineErrorOccurredEvent(Guid id, int code, string message, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(code, message, traceId, operatedAt, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    public SnackMachineErrorOccurredEvent(Guid id, int code, string message, string causedBy, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(code, message, causedBy, traceId, operatedAt, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid Id { get; } = Guid.Empty;
}
