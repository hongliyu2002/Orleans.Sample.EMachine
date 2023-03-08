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
}
