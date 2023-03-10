using Fluxera.Guards;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans.States;

[Immutable]
[GenerateSerializer]
public sealed record SnackPile
{
    public SnackPile(Guid snackId, int quantity, decimal price)
    {
        SnackId = Guard.Against.Empty(snackId, nameof(snackId));
        Quantity = Guard.Against.Negative(quantity, nameof(quantity));
        Price = Guard.Against.Negative(price, nameof(price));
        Price = Guard.Against.InvalidInput(price, x => x % 0.01m == 0, nameof(price));
    }

    [Id(0)]
    public Guid SnackId { get; }

    [Id(1)]
    public int Quantity { get; }

    [Id(2)]
    public decimal Price { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"SnackPile with SnackId:'{SnackId}' Quantity:{Quantity} Price:{Price}";
    }

    #region Create

    public static Result<SnackPile> Create(Guid snackId, int quantity, decimal price)
    {
        return Result.Ok()
                     .Verify(snackId != Guid.Empty, "Snack id cannot be empty.")
                     .Verify(quantity >= 0, "Quantity cannot be negative.")
                     .Verify(price >= 0, "Price cannot be negative.")
                     .Verify(price % 0.01m == 0, "The decimal portion of the price cannot be less than 0.01.")
                     .MapTry(() => new SnackPile(snackId, quantity, price));
    }

    #endregion

    #region Pop

    public bool TryPopOne(out SnackPile? snackPile)
    {
        if (Quantity < 1)
        {
            snackPile = null;
            return false;
        }
        snackPile = new SnackPile(SnackId, Quantity - 1, Price);
        return true;
    }

    #endregion

}
