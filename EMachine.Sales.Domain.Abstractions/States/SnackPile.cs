using Fluxera.Guards;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions.States;

[Immutable]
[GenerateSerializer]
public sealed record SnackPile
{
    public SnackPile(Snack snack, int quantity, decimal price)
    {
        Snack = Guard.Against.Null(snack, nameof(snack));
        Quantity = Guard.Against.Negative(quantity, nameof(quantity));
        Price = Guard.Against.Negative(price, nameof(price));
        Price = Guard.Against.InvalidInput(price, x => x % 0.01m > 0, nameof(price));
    }

    [Id(0)]
    public Snack Snack { get; }

    [Id(1)]
    public int Quantity { get; }

    [Id(2)]
    public decimal Price { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"SnackPile with Snack:'{Snack}' Quantity:{Quantity} Price:{Price}";
    }

    #region Create

    public static Result<SnackPile> Create(Snack snack, int quantity, decimal price)
    {
        return Result.Ok()
                     .Verify(snack != null, "Snack cannot be null.")
                     .Verify(quantity >= 0, "Quantity cannot be negative.")
                     .Verify(price >= 0, "Price cannot be negative.")
                     .Verify(price % 0.01m == 0, "The decimal portion of the price cannot be less than 0.01.")
                     .MapTry(() => new SnackPile(snack!, quantity, price));
    }

    #endregion

    #region Try Pop

    public bool TryPopOne(out SnackPile? snackPile)
    {
        if (Quantity < 1)
        {
            snackPile = null;
            return false;
        }
        snackPile = new SnackPile(Snack, Quantity - 1, Price);
        return true;
    }

    #endregion

}
