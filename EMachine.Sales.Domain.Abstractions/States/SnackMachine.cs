using EMachine.Domain.Shared;
using Fluxera.Extensions.Hosting.Modules.Domain.Shared.Model;

namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class SnackMachine : ISoftDeleteObject, IAuditedObject
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public Money MoneyInside { get; set; } = Money.Zero;

    [Id(2)]
    public decimal AmountInTransaction { get; set; }

    [Id(3)]
    public IList<Slot> Slots { get; } = new List<Slot>();

    /// <inheritdoc />
    [Id(4)]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    [Id(5)]
    public DateTimeOffset? LastModifiedAt { get; set; }

    /// <inheritdoc />
    [Id(6)]
    public DateTimeOffset? DeletedAt { get; set; }

    /// <inheritdoc />
    [Id(7)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(8)]
    public string LastModifiedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(9)]
    public string DeletedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Id(10)]
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"SnackMachine with Id:'{Id}' MoneyInside:'{MoneyInside}' AmountInTransaction:{AmountInTransaction}";
    }
}
