namespace EMachine.Orleans.Shared.States;

[GenerateSerializer]
public abstract class Repo
{
    [Id(0)]
    public HashSet<Guid> Ids { get; } = new();
}
