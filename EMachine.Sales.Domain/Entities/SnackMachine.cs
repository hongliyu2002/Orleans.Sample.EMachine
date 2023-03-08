﻿using Fluxera.Entity;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class SnackMachine : AggregateRoot<SnackMachine, Guid>, ISoftDeleteObject, IAuditedObject
{
    public Guid UuId { get; set; }

    public int Yuan1Inside { get; set; }

    public int Yuan2Inside { get; set; }

    public int Yuan5Inside { get; set; }

    public int Yuan10Inside { get; set; }

    public int Yuan20Inside { get; set; }

    public int Yuan50Inside { get; set; }

    public int Yuan100Inside { get; set; }

    public decimal AmountInside  { get; set; }

    public decimal AmountInTransaction { get; set; }

    public IList<Slot> Slots { get; set; } = new List<Slot>();

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}
