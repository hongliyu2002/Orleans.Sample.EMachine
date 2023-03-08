using EMachine.Orleans.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInsertMoneyCommand : SnackMachineCommand
{
    public SnackMachineInsertMoneyCommand(Money money, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Money = Guard.Against.Null(money);
    }

    [Id(0)]
    public Money Money { get; }
}
