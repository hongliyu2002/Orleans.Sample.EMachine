namespace EMachine.Orleans.Abstractions.States;

[GenerateSerializer]
public abstract class Repo
{
    [Id(0)]
    public HashSet<Guid> Ids { get; } = new();
}
