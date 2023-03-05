namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRemoveCommand : SnackMachineCommand
{
    public SnackMachineRemoveCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
