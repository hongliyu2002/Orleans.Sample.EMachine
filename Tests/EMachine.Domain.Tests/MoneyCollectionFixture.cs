using EMachine.Domain.Tests.Fixtures;
using Xunit;

namespace EMachine.Domain.Tests;

[CollectionDefinition(Name)]
public class MoneyCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "Money";
}
