﻿using System;
using Fluxera.Guards;
using Orleans;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class SnackNameChangedEvent : SnackEvent
{
    /// <inheritdoc />
    public SnackNameChangedEvent()
    {
        Name = string.Empty;
    }

    /// <inheritdoc />
    public SnackNameChangedEvent(Guid id, string name, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; set; }
}
