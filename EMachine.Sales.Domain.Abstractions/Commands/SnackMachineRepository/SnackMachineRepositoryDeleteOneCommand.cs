namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRepositoryDeleteOneCommand : SnackMachineRepositoryCommand
{
    public SnackMachineRepositoryDeleteOneCommand(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = id;
    }

    [Id(0)]
    public Guid Id { get; }
}
