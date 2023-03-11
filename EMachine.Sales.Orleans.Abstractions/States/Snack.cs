using EMachine.Sales.Orleans.Events;

namespace EMachine.Sales.Orleans.States;

[GenerateSerializer]
public sealed class Snack
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)]
    public string Name { get; set; } = string.Empty;

    public bool IsCreated => CreatedAt != null;

    [Id(2)]
    public DateTimeOffset? CreatedAt { get; set; }

    [Id(3)]
    public DateTimeOffset? LastModifiedAt { get; set; }

    [Id(4)]
    public DateTimeOffset? DeletedAt { get; set; }

    [Id(5)]
    public string CreatedBy { get; set; } = string.Empty;

    [Id(6)]
    public string LastModifiedBy { get; set; } = string.Empty;

    [Id(7)]
    public string DeletedBy { get; set; } = string.Empty;

    [Id(8)]
    public bool IsDeleted { get; set; }

    public override string ToString()
    {
        return $"Snack with Id:{Id} Name:'{Name}'";
    }

    #region Apply

    public void Apply(SnackInitializedEvent evt)
    {
        Id = evt.Id;
        Name = evt.Name;
        CreatedAt = evt.OperatedAt;
        CreatedBy = evt.OperatedBy;
    }

    public void Apply(SnackRemovedEvent evt)
    {
        DeletedAt = evt.OperatedAt;
        DeletedBy = evt.OperatedBy;
        IsDeleted = true;
    }

    public void Apply(SnackNameChangedEvent evt)
    {
        Name = evt.Name;
        LastModifiedAt = evt.OperatedAt;
        LastModifiedBy = evt.OperatedBy;
    }

    #endregion

}
