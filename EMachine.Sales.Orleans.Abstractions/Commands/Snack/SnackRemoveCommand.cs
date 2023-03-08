namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRemoveCommand : SnackCommand
{
    public SnackRemoveCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
