namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]

[GenerateSerializer]
public sealed class SnackRemoveCommand : SnackCommand
{
    public SnackRemoveCommand()
    {
    }

    public SnackRemoveCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
