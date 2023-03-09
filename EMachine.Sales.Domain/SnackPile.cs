﻿using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackPile
{
    public Guid SnackId { get; set; }

    public Snack Snack { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    #region Try Pop

    public bool TryPopOne()
    {
        if (Quantity < 1)
        {
            return false;
        }
        Quantity -= 1;
        return true;
    }

    #endregion

}
