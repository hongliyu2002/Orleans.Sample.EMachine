using FluentResults;
using Fluxera.Guards;
using Fluxera.ValueObject;

namespace EMachine.Sales.Domain;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class SnackPile : ValueObject<SnackPile>
{
    /// <inheritdoc />
    public SnackPile()
    {
        Snack = null!;
    }

    /// <inheritdoc />
    public SnackPile(Snack snack, int quantity, decimal price)
        : this()
    {
        Snack = Guard.Against.Null(snack, nameof(snack));
        Quantity = Guard.Against.Negative(quantity, nameof(quantity));
        Price = Guard.Against.Negative(price, nameof(price));
    }

    [Id(0)]
    public Snack Snack { get; set; }

    [Id(1)]
    public int Quantity { get; set; }

    [Id(2)]
    public decimal Price { get; set; }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Snack;
        yield return Quantity;
        yield return Price;
    }

    public bool CanPopOne()
    {
        return Quantity >= 1;
    }

    public Result<SnackPile> PopOne()
    {
        var canPopOne = CanPopOne();
        return canPopOne ? Result.Ok(new SnackPile(Snack, Quantity - 1, Price)) : Result.Fail(new Error("Insufficient snack."));
    }
}
