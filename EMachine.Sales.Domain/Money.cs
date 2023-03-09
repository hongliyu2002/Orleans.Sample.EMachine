using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class Money
{
    public int Yuan1 { get; set; }

    public int Yuan2 { get; set; }

    public int Yuan5 { get; set; }

    public int Yuan10 { get; set; }

    public int Yuan20 { get; set; }

    public int Yuan50 { get; set; }

    public int Yuan100 { get; set; }

    public decimal Amount { get; set; }

    #region Try Allocate

    public bool TryAllocate(decimal amount, out Money moneyAllocated)
    {
        if (amount < 0)
        {
            moneyAllocated = new Money();
            return false;
        }
        moneyAllocated = AllocateCore(amount);
        return moneyAllocated.Amount == amount;
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
        var money = new Money
                    {
                        Yuan1 = yuan1,
                        Yuan2 = yuan2,
                        Yuan5 = yuan5,
                        Yuan10 = yuan10,
                        Yuan20 = yuan20,
                        Yuan50 = yuan50,
                        Yuan100 = yuan100
                    };
        money.Amount = CalculateAmount(money);
        return money;
    }

    #endregion

    #region Operator

    private static decimal CalculateAmount(Money money)
    {
        return money.Yuan1 * 1m + money.Yuan2 * 2m + money.Yuan5 * 5m + money.Yuan10 * 10m + money.Yuan20 * 20m + money.Yuan50 * 50m + money.Yuan100 * 100m;
    }

    public static Money operator +(Money money1, Money money2)
    {
        var money = new Money
                    {
                        Yuan1 = money1.Yuan1 + money2.Yuan1,
                        Yuan2 = money1.Yuan2 + money2.Yuan2,
                        Yuan5 = money1.Yuan5 + money2.Yuan5,
                        Yuan10 = money1.Yuan10 + money2.Yuan10,
                        Yuan20 = money1.Yuan20 + money2.Yuan20,
                        Yuan50 = money1.Yuan50 + money2.Yuan50,
                        Yuan100 = money1.Yuan100 + money2.Yuan100
                    };
        money.Amount = CalculateAmount(money);
        return money;
    }

    public static Money operator -(Money money1, Money money2)
    {
        var money = new Money
                    {
                        Yuan1 = money1.Yuan1 - money2.Yuan1,
                        Yuan2 = money1.Yuan2 - money2.Yuan2,
                        Yuan5 = money1.Yuan5 - money2.Yuan5,
                        Yuan10 = money1.Yuan10 - money2.Yuan10,
                        Yuan20 = money1.Yuan20 - money2.Yuan20,
                        Yuan50 = money1.Yuan50 - money2.Yuan50,
                        Yuan100 = money1.Yuan100 - money2.Yuan100
                    };
        money.Amount = CalculateAmount(money);
        return money;
    }

    public static Money operator *(Money money1, int multiplier)
    {
        var money = new Money
                    {
                        Yuan1 = money1.Yuan1 * multiplier,
                        Yuan2 = money1.Yuan2 * multiplier,
                        Yuan5 = money1.Yuan5 * multiplier,
                        Yuan10 = money1.Yuan10 * multiplier,
                        Yuan20 = money1.Yuan20 * multiplier,
                        Yuan50 = money1.Yuan50 * multiplier,
                        Yuan100 = money1.Yuan100 * multiplier
                    };
        money.Amount = CalculateAmount(money);
        return money;
    }

    #endregion

}
