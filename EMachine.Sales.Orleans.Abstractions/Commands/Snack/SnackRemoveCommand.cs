namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRemoveCommand : SnackCommand
{
    public SnackRemoveCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
