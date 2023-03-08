using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterCreateOneCommand : SnackWriterCommand
{
    public SnackWriterCreateOneCommand(Guid uuId, string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        UuId = Guard.Against.Empty(uuId, nameof(uuId));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public Guid UuId { get; }

    [Id(1)]
    public string Name { get; }
}
