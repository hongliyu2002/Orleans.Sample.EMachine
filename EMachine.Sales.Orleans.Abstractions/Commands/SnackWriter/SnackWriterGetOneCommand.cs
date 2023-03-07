namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterGetOneCommand : SnackWriterCommand
{
    public SnackWriterGetOneCommand(long id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = id;
    }

    [Id(0)]
    public long Id { get; }
}
