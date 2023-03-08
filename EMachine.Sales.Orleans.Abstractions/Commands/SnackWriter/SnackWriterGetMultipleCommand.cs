using EMachine.Orleans.Shared.Commands;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterGetMultipleCommand : DomainCommand
{
    public SnackWriterGetMultipleCommand(Guid[] ids, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Ids = Guard.Against.Null(ids, nameof(ids));
    }

    [Id(0)]
    public Guid[] Ids { get; }
}
