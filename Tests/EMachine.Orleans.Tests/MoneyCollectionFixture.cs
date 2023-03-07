using EMachine.Orleans.Tests.Fixtures;
using Xunit;

namespace EMachine.Orleans.Tests;

[CollectionDefinition(Name)]
public class MoneyCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "Money";
}
