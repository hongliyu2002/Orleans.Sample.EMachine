﻿using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterGetOneCommand : SnackWriterCommand
{
    public SnackWriterGetOneCommand(Guid uuId, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        UuId = Guard.Against.Empty(uuId, nameof(uuId));
    }

    [Id(0)]
    public Guid UuId { get; }
}
