using Fluxera.Guards;

namespace EMachine.Sales.Orleans.States;

[GenerateSerializer]
public sealed class Slot
{
    public Slot(int position, SnackPile? snackPile = null)
    {
        Position = Guard.Against.Negative(position, nameof(position));
        SnackPile = snackPile;
    }

    [Id(0)]
    public int Position { get; set; }

    [Id(1)]
    public SnackPile? SnackPile { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Slot with Position:{Position} SnackPile:'{SnackPile}'";
    }
}
