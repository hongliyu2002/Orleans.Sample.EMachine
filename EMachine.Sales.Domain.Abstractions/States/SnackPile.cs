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
    }

    [Id(0)]
    public Snack Snack { get; }

    [Id(1)]
    public int Quantity { get; }

    [Id(2)]
    public decimal Price { get; }

    public bool CanPopOne()
    {
        return Quantity >= 1;
    }

    public Result<SnackPile> PopOne()
    {
        return Result.Ok()
                     .Ensure(CanPopOne(), "Insufficient snack.")
                     .Map(() => new SnackPile(Snack, Quantity - 1, Price));
    }

    #region Create

    public static Result<SnackPile> Create(Snack snack, int quantity, decimal price)
    {
        return Result.Ok()
                      // .Verify(snack is not null, "Snack cannot be null.")
                     .Verify(quantity >= 0, "Quantity cannot be negative.")
                     .Verify(price >= 0, "Price cannot be negative.")
                     .MapTry(() => new SnackPile(snack, quantity, price));
    }

    #endregion

}
