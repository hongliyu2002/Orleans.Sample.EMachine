using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineBuySnackCommand : SnackMachineCommand
{
    public SnackMachineBuySnackCommand(int position, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Position = Guard.Against.Negative(position);
    }

    [Id(0)]
    public int Position { get; }
}
