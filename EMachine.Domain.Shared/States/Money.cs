using Fluxera.Guards;
using Fluxera.ValueObject;
using Orleans.FluentResults;

namespace EMachine.Domain.Shared;

[Immutable]
[GenerateSerializer]
public sealed class Money : ValueObject<Money>
{
    public static readonly Money Zero = new(0, 0, 0, 0, 0, 0, 0);
    public static readonly Money OneYuan = new(1, 0, 0, 0, 0, 0, 0);
    public static readonly Money TwoYuan = new(0, 1, 0, 0, 0, 0, 0);
    public static readonly Money FiveYuan = new(0, 0, 1, 0, 0, 0, 0);
    public static readonly Money TenYuan = new(0, 0, 0, 1, 0, 0, 0);
    public static readonly Money TwentyYuan = new(0, 0, 0, 0, 1, 0, 0);
    public static readonly Money FiftyYuan = new(0, 0, 0, 0, 0, 1, 0);
    public static readonly Money OneHundredYuan = new(0, 0, 0, 0, 0, 0, 1);

    private Money(int yuan1, int yuan2, int yuan5, int yuan10, int yuan20, int yuan50, int yuan100)
    {
        Yuan1 = Guard.Against.Negative(yuan1, nameof(yuan1));
        Yuan2 = Guard.Against.Negative(yuan2, nameof(yuan2));
        Yuan5 = Guard.Against.Negative(yuan5, nameof(yuan5));
        Yuan10 = Guard.Against.Negative(yuan10, nameof(yuan10));
        Yuan20 = Guard.Against.Negative(yuan20, nameof(yuan20));
        Yuan50 = Guard.Against.Negative(yuan50, nameof(yuan50));
        Yuan100 = Guard.Against.Negative(yuan100, nameof(yuan100));
    }

    [Id(0)]
    public int Yuan1 { get; }
    [Id(1)]
    public int Yuan2 { get; }
    [Id(2)]
    public int Yuan5 { get; }
    [Id(3)]
    public int Yuan10 { get; }
    [Id(4)]
    public int Yuan20 { get; }
    [Id(5)]
    public int Yuan50 { get; }
    [Id(6)]
    public int Yuan100 { get; }

    public decimal Amount => Yuan1 * 1m + Yuan2 * 2m + Yuan5 * 5m + Yuan10 * 10m + Yuan20 * 20m + Yuan50 * 50m + Yuan100 * 100m;

    #region Create

    public static Result<Money> Create(int yuan1, int yuan2, int yuan5, int yuan10, int yuan20, int yuan50, int yuan100)
    {
        return Result.Ok()
                     .Verify(yuan1 >= 0, "￥1 cannot be negative.")
                     .Verify(yuan2 >= 0, "￥2 cannot be negative.")
                     .Verify(yuan5 >= 0, "￥5 cannot be negative.")
                     .Verify(yuan10 >= 0, "￥10 cannot be negative.")
                     .Verify(yuan20 >= 0, "￥20 cannot be negative.")
                     .Verify(yuan50 >= 0, "￥50 cannot be negative.")
                     .Verify(yuan100 >= 0, "￥100 cannot be negative.")
                     .MapTry(() => new Money(yuan1, yuan2, yuan5, yuan10, yuan20, yuan50, yuan100));
    }

    #endregion

    #region Allocate

    public bool CanAllocate(decimal amount, out Money allocatedMoney)
    {
        if (amount < 0)
        {
            allocatedMoney = Zero;
            return false;
        }
        allocatedMoney = AllocateCore(amount);
        return allocatedMoney.Amount == amount;
    }

    public Result<Money> Allocate(decimal amount)
    {
        return Result.Ok()
                     .Ensure(amount >= 0, $"Invalid amount: {amount}.")
                     .Ensure(CanAllocate(amount, out var allocatedMoney), "Cannot allocate money, lack of change.")
                     .Map(() => allocatedMoney);
    }

    private Money AllocateCore(decimal amount)
    {
        var yuan100 = Math.Min((int)(amount / 100m), Yuan100);
        amount -= yuan100 * 100m;
        var yuan50 = Math.Min((int)(amount / 50m), Yuan50);
        amount -= yuan50 * 50m;
        var yuan20 = Math.Min((int)(amount / 20m), Yuan20);
        amount -= yuan20 * 20m;
        var yuan10 = Math.Min((int)(amount / 10m), Yuan10);
        amount -= yuan10 * 10m;
        var yuan5 = Math.Min((int)(amount / 5m), Yuan5);
        amount -= yuan5 * 5m;
        var yuan2 = Math.Min((int)(amount / 2m), Yuan2);
        amount -= yuan2 * 2m;
        var yuan1 = Math.Min((int)(amount / 1m), Yuan1);
        // amount -= yuan1 * 1m;
        return new Money(yuan1, yuan2, yuan5, yuan10, yuan20, yuan50, yuan100);
    }

    #endregion

    #region Operator

    public static Money operator +(Money money1, Money money2)
    {
        money1 = Guard.Against.Null(money1);
        money2 = Guard.Against.Null(money2);
        var result = Create(money1.Yuan1 + money2.Yuan1, money1.Yuan2 + money2.Yuan2, money1.Yuan5 + money2.Yuan5, money1.Yuan10 + money2.Yuan10, money1.Yuan20 + money2.Yuan20,
                            money1.Yuan50 + money2.Yuan50, money1.Yuan100 + money2.Yuan100);
        return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.ToString());
    }

    public static Money operator -(Money money1, Money money2)
    {
        money1 = Guard.Against.Null(money1);
        money2 = Guard.Against.Null(money2);
        var result = Create(money1.Yuan1 - money2.Yuan1, money1.Yuan2 - money2.Yuan2, money1.Yuan5 - money2.Yuan5, money1.Yuan10 - money2.Yuan10, money1.Yuan20 - money2.Yuan20,
                            money1.Yuan50 - money2.Yuan50, money1.Yuan100 - money2.Yuan100);
        return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.ToString());
    }

    public static Money operator *(Money money1, int multiplier)
    {
        money1 = Guard.Against.Null(money1);
        multiplier = Guard.Against.Negative(multiplier, nameof(multiplier));
        var result = Create(money1.Yuan1 * multiplier, money1.Yuan2 * multiplier, money1.Yuan5 * multiplier, money1.Yuan10 * multiplier, money1.Yuan20 * multiplier,
                            money1.Yuan50 * multiplier, money1.Yuan100 * multiplier);
        return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.ToString());
    }

    #endregion

}
