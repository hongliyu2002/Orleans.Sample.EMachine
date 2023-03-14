namespace EMachine.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public abstract record DomainCommand(Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) : ITraceable;
