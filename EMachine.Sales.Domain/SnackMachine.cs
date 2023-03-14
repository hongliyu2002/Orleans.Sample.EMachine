using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class SnackMachine
{
    public Guid Id { get; set; }

    public Money MoneyInside { get; set; } = null!;

    public decimal AmountInTransaction { get; set; }

    public IList<Slot> Slots { get; set; } = new List<Slot>();

    public int SlotsCount { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastModifiedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }

    public long Version { get; set; }
}
