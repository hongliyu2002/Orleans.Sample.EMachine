using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Domain.Tests;

[Collection(SnackCollectionFixture.Name)]
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
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(999);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Orange", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(999);
        getResult.Value.Name.Should().Be("Orange");
        getResult.Value.CreatedBy.Should().Be("Leo");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_ChangeName_And_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(1000);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Mongo", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var changeNameResult = await grain.ChangeNameAsync(new SnackNameChangeCommand("Candy", Guid.NewGuid(), "Boss"));
        changeNameResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(1000);
        getResult.Value.Name.Should().Be("Candy");
        getResult.Value.CreatedBy.Should().Be("Leo");
        getResult.Value.LastModifiedBy.Should().Be("Boss");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_Remove_And_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(1001);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("BBQ", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(false);
        getResult.Value.Id.Should().Be(1001);
        getResult.Value.Name.Should().Be("BBQ");
        getResult.Value.CreatedBy.Should().Be("Leo");
        getResult.Value.IsDeleted.Should().Be(true);
        getResult.Value.DeletedBy.Should().Be("Boss");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Reinitialize_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(1002);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Red Tea", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Green Tea", Guid.NewGuid(), "Boss"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _testOutputHelper.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(1002);
        getResult.Value.Name.Should().Be("Red Tea");
        getResult.Value.CreatedBy.Should().Be("Leo");
        _testOutputHelper.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Initialize_Snack_When_Deleted()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(1003);
        var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Chocolate", Guid.NewGuid(), "Janet"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _testOutputHelper.WriteLine(reInitializeResult.ToString());
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(false);
        getResult.Value.Id.Should().Be(1003);
        getResult.Value.Name.Should().Be("Cafe");
        getResult.Value.CreatedBy.Should().Be("Leo");
        getResult.Value.IsDeleted.Should().Be(true);
        getResult.Value.DeletedBy.Should().Be("Boss");
        _testOutputHelper.WriteLine(getResult.ToString());
    }
}
