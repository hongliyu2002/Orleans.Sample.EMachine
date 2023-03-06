namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineUnloadMoneyCommand : SnackMachineCommand
{
    public SnackMachineUnloadMoneyCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
