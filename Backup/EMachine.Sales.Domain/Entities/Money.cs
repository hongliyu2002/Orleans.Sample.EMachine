using Fluxera.ValueObject;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class Money : ValueObject<Money>
{
    public int Yuan1 { get; set; }

    public int Yuan2 { get; set; }

    public int Yuan5 { get; set; }

    public int Yuan10 { get; set; }

    public int Yuan20 { get; set; }

    public int Yuan50 { get; set; }

    public int Yuan100 { get; set; }

    public decimal Amount => Yuan1 * 1m + Yuan2 * 2m + Yuan5 * 5m + Yuan10 * 10m + Yuan20 * 20m + Yuan50 * 50m + Yuan100 * 100m;
}
