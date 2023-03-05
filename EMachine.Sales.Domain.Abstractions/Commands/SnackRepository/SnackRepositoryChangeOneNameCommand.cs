using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRepositoryChangeOneNameCommand : SnackRepositoryCommand
{
    public SnackRepositoryChangeOneNameCommand(long id, string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public long Id { get; }

    [Id(1)]
    public string Name { get; }
}
