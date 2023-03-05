using Fluxera.Guards;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions.States;

[Immutable]
[GenerateSerializer]
public sealed record Slot
{
    public Slot(Guid machineId, SnackPile snackPile, int position)
    {
        MachineId = machineId;
        SnackPile = Guard.Against.Null(snackPile, nameof(snackPile));
        Position = Guard.Against.Negative(position, nameof(position));
    }

    [Id(0)]
    public Guid MachineId { get; }

    [Id(1)]
    public SnackPile SnackPile { get; }

    [Id(2)]
    public int Position { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Slot with MachineId:'{MachineId}' SnackPile:'{SnackPile}' Position:{Position}";
    }

    #region Create

    public static Result<Slot> Create(Guid machineId, SnackPile snackPile, int position)
    {
        return Result.Ok()
                     .Verify(snackPile is not null, "Snack Pile cannot be null.")
                     .Verify(position >= 0, "Position cannot be negative.")
                     .MapTry(() => new Slot(machineId, snackPile!, position));
    }

    #endregion

}
