﻿namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class Slot
{
    [Id(0)]
    public Guid MachineId { get; set; }

    [Id(1)]
    public SnackPile? SnackPile { get; set; }

    [Id(2)]
    public int Position { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Slot with MachineId:'{MachineId}' SnackPile:'{SnackPile}' Position:{Position}";
    }
}
