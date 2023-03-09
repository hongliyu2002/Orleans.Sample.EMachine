using EMachine.Sales.Orleans.Tests.Fixtures;
using Xunit;

namespace EMachine.Sales.Orleans.Tests.Projections;

[CollectionDefinition(Name)]
public class SnackProjectionCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "SnackProjection";
}
