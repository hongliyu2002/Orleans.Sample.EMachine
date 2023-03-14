using System.Collections.Immutable;

namespace EMachine.Orleans.Abstractions.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainListQuery(IImmutableList<KeyValuePair<string, bool>> Sorts, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) 
    : DomainQuery(TraceId, OperatedAt, OperatedBy);
