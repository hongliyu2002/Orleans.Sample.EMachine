using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackWriterCreateOneCommand : SnackWriterCommand
{
    public SnackWriterCreateOneCommand(Guid key, string name, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public Guid Key { get; }

    [Id(1)]
    public string Name { get; }
}
