using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Domain.Tests;

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
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Cafe");
        getResult.Value.CreatedBy.Should().Be("Leo");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_ChangeName_And_Get_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var changeNameResult = await grain.ChangeNameAsync(new SnackNameChangeCommand("Chocolate", Guid.NewGuid(), "Boss"));
        changeNameResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Chocolate");
        getResult.Value.CreatedBy.Should().Be("Leo");
        getResult.Value.LastModifiedBy.Should().Be("Boss");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_Remove_And_Get_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(false);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Cafe");
        getResult.Value.CreatedBy.Should().Be("Leo");
        getResult.Value.IsDeleted.Should().Be(true);
        getResult.Value.DeletedBy.Should().Be("Boss");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Reinitialize_Snack()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Chocolate", Guid.NewGuid(), "Leo"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _testOutputHelper.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Cafe");
        getResult.Value.CreatedBy.Should().Be("Leo");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Initialize_Snack_When_Deleted()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(id);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Chocolate", Guid.NewGuid(), "Leo"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _testOutputHelper.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(false);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Name.Should().Be("Cafe");
        getResult.Value.CreatedBy.Should().Be("Leo");
        getResult.Value.IsDeleted.Should().Be(true);
        getResult.Value.DeletedBy.Should().Be("Boss");
        _testOutputHelper.WriteLine(getResult.ToString());
    }
}
