using FluentResults;
using Fluxera.Guards;
using Fluxera.ValueObject;

namespace EMachine.Domain.Shared;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class MoneyInventory : ValueObject<MoneyInventory>
{
    public static readonly MoneyInventory Zero = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory OneFen = new(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory TwoFen = new(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory FiveFen = new(0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory OneJiao = new(0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory TwoJiao = new(0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory FiveJiao = new(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory OneYuan = new(0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory TwoYuan = new(0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
    public static readonly MoneyInventory FiveYuan = new(0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
    public static readonly MoneyInventory TenYuan = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
    public static readonly MoneyInventory TwentyYuan = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
    public static readonly MoneyInventory FiftyYuan = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
    public static readonly MoneyInventory OneHundredYuan = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);

    public MoneyInventory()
    {
    }

    /// <inheritdoc />
    public MoneyInventory(int fen1, int fen2, int fen5, int jiao1, int jiao2, int jiao5, int yuan1, int yuan2, int yuan5, int yuan10, int yuan20, int yuan50, int yuan100)
        : this()
    {
        Fen1 = Guard.Against.Negative(fen1, nameof(fen1));
        Fen2 = Guard.Against.Negative(fen2, nameof(fen2));
        Fen5 = Guard.Against.Negative(fen5, nameof(fen5));
        Jiao1 = Guard.Against.Negative(jiao1, nameof(jiao1));
        Jiao2 = Guard.Against.Negative(jiao2, nameof(jiao2));
        Jiao5 = Guard.Against.Negative(jiao5, nameof(jiao5));
        Yuan1 = Guard.Against.Negative(yuan1, nameof(yuan1));
        Yuan2 = Guard.Against.Negative(yuan2, nameof(yuan2));
        Yuan5 = Guard.Against.Negative(yuan5, nameof(yuan5));
        Yuan10 = Guard.Against.Negative(yuan10, nameof(yuan10));
        Yuan20 = Guard.Against.Negative(yuan20, nameof(yuan20));
        Yuan50 = Guard.Against.Negative(yuan50, nameof(yuan50));
        Yuan100 = Guard.Against.Negative(yuan100, nameof(yuan100));
    }

    [Id(0)]
    public int Fen1 { get; set; }
    [Id(1)]
    public int Fen2 { get; set; }
    [Id(2)]
    public int Fen5 { get; set; }
    [Id(3)]
    public int Jiao1 { get; set; }
    [Id(4)]
    public int Jiao2 { get; set; }
    [Id(5)]
    public int Jiao5 { get; set; }
    [Id(6)]
    public int Yuan1 { get; set; }
    [Id(7)]
    public int Yuan2 { get; set; }
    [Id(8)]
    public int Yuan5 { get; set; }
    [Id(9)]
    public int Yuan10 { get; set; }
    [Id(10)]
    public int Yuan20 { get; set; }
    [Id(11)]
    public int Yuan50 { get; set; }
    [Id(12)]
    public int Yuan100 { get; set; }

    public static MoneyInventory operator +(MoneyInventory money1, MoneyInventory money2)
    {
        return new MoneyInventory(money1.Fen1 + money2.Fen1, money1.Fen2 + money2.Fen2, money1.Fen5 + money2.Fen5, money1.Jiao1 + money2.Jiao1, money1.Jiao2 + money2.Jiao2,
                                  money1.Jiao5 + money2.Jiao5, money1.Yuan1 + money2.Yuan1, money1.Yuan2 + money2.Yuan2, money1.Yuan5 + money2.Yuan5, money1.Yuan10 + money2.Yuan10,
                                  money1.Yuan20 + money2.Yuan20, money1.Yuan50 + money2.Yuan50, money1.Yuan100 + money2.Yuan100);
    }

    public static MoneyInventory operator -(MoneyInventory money1, MoneyInventory money2)
    {
        return new MoneyInventory(money1.Fen1 - money2.Fen1, money1.Fen2 - money2.Fen2, money1.Fen5 - money2.Fen5, money1.Jiao1 - money2.Jiao1, money1.Jiao2 - money2.Jiao2,
                                  money1.Jiao5 - money2.Jiao5, money1.Yuan1 - money2.Yuan1, money1.Yuan2 - money2.Yuan2, money1.Yuan5 - money2.Yuan5, money1.Yuan10 - money2.Yuan10,
                                  money1.Yuan20 - money2.Yuan20, money1.Yuan50 - money2.Yuan50, money1.Yuan100 - money2.Yuan100);
    }

    public static MoneyInventory operator *(MoneyInventory money1, int multiplier)
    {
        multiplier = Guard.Against.Negative(multiplier, nameof(multiplier));
        return new MoneyInventory(money1.Fen1 * multiplier, money1.Fen2 * multiplier, money1.Fen5 * multiplier, money1.Jiao1 * multiplier, money1.Jiao2 * multiplier,
                                  money1.Jiao5 * multiplier, money1.Yuan1 * multiplier, money1.Yuan2 * multiplier, money1.Yuan5 * multiplier, money1.Yuan10 * multiplier,
                                  money1.Yuan20 * multiplier, money1.Yuan50 * multiplier, money1.Yuan100 * multiplier);
    }

    public decimal GetAmount()
    {
        return Fen1 * 0.01m + Fen2 * 0.02m + Fen5 * 0.05m + Jiao1 * 0.1m + Jiao2 * 0.2m + Jiao5 * 0.5m + Yuan1 * 1m + Yuan2 * 2m + Yuan5 * 5m + Yuan10 * 10m + Yuan20 * 20m
             + Yuan50 * 50m + Yuan100 * 100m;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Fen1;
        yield return Fen2;
        yield return Fen5;
        yield return Jiao1;
        yield return Jiao2;
        yield return Jiao5;
        yield return Yuan1;
        yield return Yuan2;
        yield return Yuan5;
        yield return Yuan10;
        yield return Yuan20;
        yield return Yuan50;
        yield return Yuan100;
    }

    public bool CanAllocate(decimal amount, out MoneyInventory allocatedMoney)
    {
        amount = Guard.Against.NegativeOrZero(amount);
        allocatedMoney = AllocateCore(amount);
        return allocatedMoney.GetAmount() == amount;
    }

    public Result<MoneyInventory> Allocate(decimal amount)
    {
        amount = Guard.Against.NegativeOrZero(amount);
        var canAllocate = CanAllocate(amount, out var allocatedMoney);
        return canAllocate ? Result.Ok(allocatedMoney) : Result.Fail(new Error("Lack of change in money inventory."));
    }

    private MoneyInventory AllocateCore(decimal amount)
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
        amount -= yuan1 * 1m;
        var jiao5 = Math.Min((int)(amount / 0.5m), Jiao5);
        amount -= jiao5 * 0.5m;
        var jiao2 = Math.Min((int)(amount / 0.2m), Jiao2);
        amount -= jiao2 * 0.2m;
        var jiao1 = Math.Min((int)(amount / 0.1m), Jiao1);
        amount -= jiao1 * 0.1m;
        var fen5 = Math.Min((int)(amount / 0.05m), Fen5);
        amount -= fen5 * 0.05m;
        var fen2 = Math.Min((int)(amount / 0.02m), Fen2);
        amount -= fen2 * 0.02m;
        var fen1 = Math.Min((int)(amount / 0.01m), Fen1);
        return new MoneyInventory(fen1, fen2, fen5, jiao1, jiao2, jiao5, yuan1, yuan2, yuan5, yuan10, yuan20, yuan50, yuan100);
    }
}
