﻿using Fluxera.Entity;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class Slot : Entity<Slot, Guid>
{
    public Guid MachineUuId { get; set; }

    public int Position { get; set; }

    public Guid? SnackUuId { get; set; }

    public Snack? Snack { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }
}
