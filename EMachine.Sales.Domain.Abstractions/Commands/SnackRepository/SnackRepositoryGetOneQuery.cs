﻿using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRepositoryGetOneQuery : SnackRepositoryQuery
{
    public SnackRepositoryGetOneQuery(long id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
    }

    [Id(0)]
    public long Id { get; }
}