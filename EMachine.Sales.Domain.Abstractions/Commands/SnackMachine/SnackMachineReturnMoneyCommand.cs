using EMachine.Domain.Shared;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineReturnMoneyCommand : SnackMachineCommand
{
    public SnackMachineReturnMoneyCommand(Money money, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
    }
}
