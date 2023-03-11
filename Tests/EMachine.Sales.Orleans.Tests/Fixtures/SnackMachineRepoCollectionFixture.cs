using Xunit;

namespace EMachine.Sales.Orleans.Tests.Fixtures;

[CollectionDefinition(Name)]
public class TestCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "Sales";
}
