using Fluxera.ValueObject;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class SnackPile : ValueObject<SnackPile>
{
    public Guid SnackUuId { get; set; }

    public Snack Snack { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
