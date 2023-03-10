using EMachine.Sales.Orleans.Tests.Fixtures;
using Xunit;

namespace EMachine.Sales.Orleans.Tests;

[CollectionDefinition(Name)]
public class SnackMachineCollectionFixture : ICollectionFixture<ClusterFixture>
{
    public const string Name = "SnackMachine";
}
