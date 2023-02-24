namespace EMachine.Domain.Shared;

public interface ITraceable
{
    Guid TraceId { get; }

    public string OperatedBy { get; }
}
