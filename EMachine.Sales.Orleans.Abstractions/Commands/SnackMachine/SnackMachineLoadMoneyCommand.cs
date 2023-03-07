using EMachine.Orleans.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineLoadMoneyCommand : SnackMachineCommand
{
    public SnackMachineLoadMoneyCommand(Money money, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Money = Guard.Against.Null(money);
    }

    [Id(0)]
    public Money Money { get; }
}
