using Fluxera.Guards;

namespace EMachine.Orleans.Shared.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainQuery : ITraceable
{
    protected DomainQuery(Guid traceId, DateTimeOffset operatedAt, string operatedBy)
    {
        TraceId = Guard.Against.Empty(traceId, nameof(traceId));
        OperatedAt = Guard.Against.Default(operatedAt, nameof(operatedAt));
        OperatedBy = Guard.Against.NullOrWhiteSpace(operatedBy, nameof(operatedBy));
    }

    [Id(0)]
    public Guid TraceId { get; } = Guid.Empty;

    [Id(1)]
    public DateTimeOffset OperatedAt { get; } = DateTimeOffset.UtcNow;

    [Id(2)]
    public string OperatedBy { get; } = string.Empty;
}
