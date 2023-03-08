using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackPile
{
    public Guid SnackKey { get; set; }

    public Snack Snack { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
