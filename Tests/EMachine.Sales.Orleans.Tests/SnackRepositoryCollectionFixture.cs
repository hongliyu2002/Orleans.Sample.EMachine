using EMachine.Sales.Orleans.Tests.Fixtures;
using Xunit;

namespace EMachine.Sales.Orleans.Tests;

[CollectionDefinition(Name)]
public class SnackRepositoryCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "SnackRepository";
}
