using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Domain.Tests;

[Collection(TestCollectionFixture.Name)]
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
        var id = new Guid("0edbbe28-116d-4c0e-9f64-baebb91f7500");
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var createResult = await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(id, "Apple", Guid.NewGuid(), "Leo"));
        createResult.IsSuccess.Should().Be(true);
        createResult.Value.Id.Should().Be(id);
        createResult.Value.Name.Should().Be("Apple");
        createResult.Value.CreatedBy.Should().Be("Leo");
        _testOutputHelper.WriteLine(createResult.ToString());
    }
    
    [Fact]
    public async Task Can_Delete_Snack()
    {
        var id = new Guid("13ba54cc-ae13-4de8-a460-d9fcd17a3674");
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var createResult = await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(id, "Lemon", Guid.NewGuid(), "Leo"));
        createResult.IsSuccess.Should().Be(true);
        var deleteResult = await grain.DeleteSnackAsync(new SnackRepositoryDeleteOneCommand(id, Guid.NewGuid(), "Boss"));
        deleteResult.IsSuccess.Should().Be(true);
        _testOutputHelper.WriteLine(deleteResult.ToString());
    }

    [Fact]
    public async Task Can_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        var getResult = await grain.GetSnackAsync(new SnackRepositoryGetOneCommand(new Guid("a74ad750-0d8b-4ede-ad29-e5e0d5c2dd9e"), Guid.NewGuid(), "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Id.Should().Be(new Guid("a74ad750-0d8b-4ede-ad29-e5e0d5c2dd9e"));
        getResult.Value.Name.Should().Be("Cafe");
    }

    [Fact]
    public async Task Can_Get_Snacks()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackRepositoryGrain>(Guid.Empty);
        // await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(new Guid("a74ad750-0d8b-4ede-ad29-e5e0d5c2dd9e"), "Cafe", Guid.NewGuid(), "Leo"));
        // await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(new Guid("7db1925a-4458-479f-9c6b-1f708c35b29e"), "Chocolate", Guid.NewGuid(), "Leo"));
        // await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(new Guid("d5753343-78d4-4c4a-a674-63b75f096059"), "Soda", Guid.NewGuid(), "Leo"));
        // await grain.CreateSnackAsync(new SnackRepositoryCreateOneCommand(new Guid("83d423b6-96a3-48fd-b9e7-97a00bb98e2f"), "Gum", Guid.NewGuid(), "Leo"));
        var getResult = await grain.GetSnacksAsync(new SnackRepositoryGetListCommand(100, 0, Guid.NewGuid(), "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Count.Should().BeGreaterOrEqualTo(4);
        getResult.Value.ForEach(x => _testOutputHelper.WriteLine(x.ToString()));
    }
}
