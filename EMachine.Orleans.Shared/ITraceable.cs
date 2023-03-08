namespace EMachine.Orleans.Shared;

public interface ITraceable
{
    Guid TraceId { get; }

    public DateTimeOffset OperatedAt { get; }

    public string OperatedBy { get; }
}
