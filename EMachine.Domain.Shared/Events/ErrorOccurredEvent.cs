using System.Collections.Immutable;
using Fluxera.Guards;

namespace EMachine.Domain.Shared.Events;

[Immutable]
[GenerateSerializer]
public abstract record ErrorOccurredEvent : DomainEvent
{
    protected ErrorOccurredEvent(int code, string message, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Code = code;
        Message = Guard.Against.NullOrWhiteSpace(message, nameof(message));
        Reasons = ImmutableList<string>.Empty;
    }

    protected ErrorOccurredEvent(int code, string message, string causedBy, Guid traceId, string operatedBy)
        : this(code, message, traceId, operatedBy)
    {
        causedBy = Guard.Against.NullOrWhiteSpace(causedBy, nameof(causedBy));
        Reasons = Reasons.Add(causedBy);
    }

    [Id(0)]
    public int Code { get; }
    [Id(1)]
    public string Message { get; } = string.Empty;
    [Id(2)]
    public IImmutableList<string> Reasons { get; } = ImmutableList<string>.Empty;
}
