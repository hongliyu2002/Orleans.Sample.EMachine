namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class SnackRepository
{
    [Id(0)]
    public HashSet<Guid> Set { get; set; } = new();
}
