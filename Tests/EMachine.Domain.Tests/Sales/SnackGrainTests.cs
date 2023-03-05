using EMachine.Domain.Tests.Fixtures;
using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Domain.Tests.Sales;

[Collection(TestCollectionFixture.Name)]
public class SnackGrainTests
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _testOutputHelper;

    public SnackGrainTests(ClusterFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Initialize_And_Get_Snack()
    {
        var id = Guid.NewGuid();
        var name = "Cafe";
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var result = await grain.InitializeAsync(new SnackInitializeCommand(name, Guid.NewGuid(), "Leo"));
        result.IsSuccess.Should()
              .Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should()
                 .Be(true);
        getResult.Value.Id.Should()
                 .Be(id);
        getResult.Value.Name.Should()
                 .Be(name);
        _testOutputHelper.WriteLine(getResult.ToString());
    }
}
