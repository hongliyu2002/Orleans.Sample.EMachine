using Fluxera.Guards;

namespace EMachine.Domain.Shared.Events;

[Immutable]
[Serializable]
[GenerateSerializer]
public abstract class ErrorOccurredEvent : DomainEvent
{
    protected ErrorOccurredEvent()
    {
        Message = string.Empty;
        Reasons = new List<string>();
    }

    protected ErrorOccurredEvent(int code, string message, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Message = Guard.Against.NullOrWhiteSpace(message, nameof(message));
        Reasons = new List<string>();
    }

    protected ErrorOccurredEvent(int code, string message, string causedBy, Guid traceId, string operatedBy)
        : this(code, message, traceId, operatedBy)
    {
        causedBy = Guard.Against.NullOrWhiteSpace(causedBy, nameof(causedBy));
        Reasons.Add(causedBy);
    }

    [Id(0)]
    public int Code { get; set; }
    [Id(1)]
    public string Message { get; set; }
    [Id(2)]
    public List<string> Reasons { get; set; }
}
