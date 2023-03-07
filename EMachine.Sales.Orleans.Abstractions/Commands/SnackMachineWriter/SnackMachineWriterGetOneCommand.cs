namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterGetOneCommand : SnackMachineWriterCommand
{
    public SnackMachineWriterGetOneCommand(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = id;
    }

    [Id(0)]
    public Guid Id { get; }
}
