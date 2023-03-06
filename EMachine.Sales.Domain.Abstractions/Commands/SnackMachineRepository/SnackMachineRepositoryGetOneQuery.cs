namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRepositoryGetOneQuery : SnackMachineRepositoryQuery
{
    public SnackMachineRepositoryGetOneQuery(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = id;
    }

    [Id(0)]
    public Guid Id { get; }
}
