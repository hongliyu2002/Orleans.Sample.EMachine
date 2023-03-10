namespace EMachine.Sales.Orleans.Views;

[Immutable]
[GenerateSerializer]
public sealed record SnackBasic(Guid Id, string Name);
