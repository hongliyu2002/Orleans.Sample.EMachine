﻿using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackInitializeCommand : SnackCommand
{
    public SnackInitializeCommand(string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; }
}