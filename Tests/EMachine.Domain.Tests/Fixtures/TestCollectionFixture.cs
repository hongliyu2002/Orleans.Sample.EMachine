using Xunit;

namespace EMachine.Domain.Tests.Fixtures;

[CollectionDefinition(Name)]
public class TestCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "ClusterCollection";
}
