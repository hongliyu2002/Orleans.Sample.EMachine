using EMachine.Orleans.Shared.Commands;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineBuySnackCommand : DomainCommand
{
    public SnackMachineBuySnackCommand(int position, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Position = Guard.Against.Negative(position);
    }

    [Id(0)]
    public int Position { get; }
}
