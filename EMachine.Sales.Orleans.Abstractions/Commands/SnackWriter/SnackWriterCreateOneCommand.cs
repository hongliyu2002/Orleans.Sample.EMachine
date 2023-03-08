﻿using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterCreateOneCommand : SnackWriterCommand
{
    public SnackWriterCreateOneCommand(Guid id, string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public Guid Id { get; }

    [Id(1)]
    public string Name { get; }
}