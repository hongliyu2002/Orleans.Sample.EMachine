using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Domain.Tests;

[Collection(SnackRepositoryCollectionFixture.Name)]
public class SnackRepositoryGainTests
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _testOutputHelper;

    public SnackRepositoryGainTests(ClusterFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Create_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var createResult = await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(1000, "Apple", Guid.NewGuid(), "Leo"));
        createResult.IsSuccess.Should().Be(true);
        var result = await createResult.Value.GetAsync();
        result.IsSuccess.Should().Be(true);
        result.Value.Id.Should().Be(1000);
        result.Value.Name.Should().Be("Apple");
        result.Value.CreatedBy.Should().Be("Leo");
        _testOutputHelper.WriteLine(createResult.ToString());
    }

    [Fact]
    public async Task Can_Delete_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var createResult = await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(1001, "Lemon", Guid.NewGuid(), "Leo"));
        createResult.IsSuccess.Should().Be(true);
        var deleteResult = await grain.DeleteSnackAsync(new SnackRepositoryDeleteOneCommand(1001, Guid.NewGuid(), "Boss"));
        deleteResult.IsSuccess.Should().Be(true);
        _testOutputHelper.WriteLine(deleteResult.ToString());
    }

    [Fact]
    public async Task Can_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var getResult = await grain.GetSnackAsync(new SnackRepositoryGetOneQuery(1, Guid.NewGuid(), "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        var result = await getResult.Value.GetAsync();
        result.Value.Id.Should().Be(1);
        result.Value.Name.Should().Be("Cafe");
    }

    [Fact]
    public async Task Can_Get_Snacks()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var getResult = await grain.GetSnacksAsync(new SnackRepositoryGetListQuery(100, 0, Guid.NewGuid(), "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Count.Should().BeGreaterOrEqualTo(5);
        getResult.Value.ForEach(x => _testOutputHelper.WriteLine(x.ToString()));
    }
}
