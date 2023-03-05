namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class SnackRepository
{
    [Id(0)]
    public HashSet<long> Set { get; } = new();
}
