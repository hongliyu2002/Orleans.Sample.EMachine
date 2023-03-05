using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRepositoryGetOneCommand : SnackRepositoryCommand
{
    public SnackRepositoryGetOneCommand(long id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
    }

    [Id(0)]
    public long Id { get; }
}
