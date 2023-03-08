﻿using EMachine.Orleans.Shared.Commands;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterCreateOneCommand : DomainCommand
{
    public SnackWriterCreateOneCommand(Guid id, string name, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public Guid Id { get; } = Guid.Empty;

    [Id(1)]
    public string Name { get; }
}
