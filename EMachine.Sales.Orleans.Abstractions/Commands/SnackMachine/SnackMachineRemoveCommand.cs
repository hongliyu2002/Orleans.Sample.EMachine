using EMachine.Orleans.Shared.Commands;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRemoveCommand : DomainCommand
{
    public SnackMachineRemoveCommand(Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
    }
}
