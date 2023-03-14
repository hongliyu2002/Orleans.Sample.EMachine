namespace EMachine.Orleans.Abstractions.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainQuery(Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) : ITraceable;
