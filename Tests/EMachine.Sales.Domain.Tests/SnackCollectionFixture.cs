using EMachine.Sales.Domain.Tests.Fixtures;
using Xunit;

namespace EMachine.Sales.Domain.Tests;

[CollectionDefinition(Name)]
public class SnackCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "Snack";
}
