namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineReturnMoneyCommand : SnackMachineCommand
{
    public SnackMachineReturnMoneyCommand(Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
