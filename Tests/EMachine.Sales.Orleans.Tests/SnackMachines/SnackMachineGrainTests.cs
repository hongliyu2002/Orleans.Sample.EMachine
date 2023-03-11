using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.States;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(TestCollectionFixture.Name)]
public class SnackMachineGrainTests : IClassFixture<SnackMachineFixture>
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public SnackMachineGrainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task Can_Initialize_And_Get_SnackMachine()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackMachineGrain>(id);
        var cafeSnackId = new Guid("ae9e8d38-8289-47fe-8084-99df2b894556");
        var chocolateSnackId = new Guid("23697d49-75f1-4e3c-aa0d-5a98cf3ad122");
        var slots = ImmutableList.Create(new Slot(0), new Slot(1, new SnackPile(cafeSnackId, 10, 5)), new Slot(2), new Slot(3, new SnackPile(chocolateSnackId, 20, 8)));
        var initializeResult = await grain.InitializeAsync(new SnackMachineInitializeCommand(Money.OneHundredYuan, slots, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        await Task.Delay(1000);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Slots.Should().BeEquivalentTo(slots);
        await Task.Delay(1000);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Cannot_Reinitialize_SnackMachine()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackMachineGrain>(id);
        var cafeSnackId = new Guid("ae9e8d38-8289-47fe-8084-99df2b894556");
        var chocolateSnackId = new Guid("23697d49-75f1-4e3c-aa0d-5a98cf3ad122");
        var slots = ImmutableList.Create(new Slot(0), new Slot(1, new SnackPile(cafeSnackId, 10, 5)), new Slot(2), new Slot(3, new SnackPile(chocolateSnackId, 20, 8)));
        var initializeResult = await grain.InitializeAsync(new SnackMachineInitializeCommand(Money.OneHundredYuan, slots, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var reInitializeResult = await grain.InitializeAsync(new SnackMachineInitializeCommand(new Money(70, 60, 50, 40, 30, 20, 10), slots, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        reInitializeResult.IsSuccess.Should().Be(false);
        _output.WriteLine(reInitializeResult.ToString());
        await Task.Delay(1000);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.MoneyInside.Should().Be(Money.OneHundredYuan);
        getResult.Value.Slots.Should().BeEquivalentTo(slots);
        await Task.Delay(1000);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_Remove_SnackMachine()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackMachineGrain>(id);
        var sodaSnackId = new Guid("5b4103f4-7d90-4680-afc1-70dc48b96629");
        var slots = ImmutableList.Create(new Slot(0), new Slot(1, new SnackPile(sodaSnackId, 10, 5)), new Slot(2));
        var initializeResult = await grain.InitializeAsync(new SnackMachineInitializeCommand(new Money(70, 60, 50, 40, 30, 20, 10), slots, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var removeResult = await grain.RemoveAsync(new SnackMachineRemoveCommand(id, DateTimeOffset.UtcNow, "Boss"));
        removeResult.IsSuccess.Should().Be(true);
        await Task.Delay(1000);
        var getResult = await grain.GetAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Id.Should().Be(id);
        getResult.Value.Slots.Should().BeEquivalentTo(slots);
        getResult.Value.IsDeleted.Should().Be(true);
        await Task.Delay(1000);
        _output.WriteLine(getResult.ToString());
    }

    [Fact]
    public async Task Can_InsertMoney_And_Get_AmountInTransaction()
    {
        var id = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<ISnackMachineGrain>(id);
        var sodaSnackId = new Guid("5b4103f4-7d90-4680-afc1-70dc48b96629");
        var slots = ImmutableList.Create(new Slot(0), new Slot(1, new SnackPile(sodaSnackId, 10, 5)), new Slot(2));
        var initializeResult = await grain.InitializeAsync(new SnackMachineInitializeCommand(new Money(70, 60, 50, 40, 30, 20, 10), slots, Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
        initializeResult.IsSuccess.Should().Be(true);
        var insertMoneyResult = await grain.InsertMoneyAsync(new SnackMachineInsertMoneyCommand(Money.TenYuan, Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        insertMoneyResult.IsSuccess.Should().Be(true);
        await Task.Delay(1000);
        var getResult = await grain.GetAmountInTransactionAsync();
        getResult.IsSuccess.Should().Be(true);
        getResult.Value.Should().Be(10);
        await Task.Delay(1000);
        _output.WriteLine(getResult.ToString());
    }

    //
    // [Fact]
    // public async Task Cannot_Initialize_Snack_When_Deleted()
    // {
    //     var grain = _cluster.GrainFactory.GetGrain<ISnackGrain>(Guid.NewGuid());
    //     var initializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Cafe", Guid.NewGuid(), DateTimeOffset.UtcNow, "Leo"));
    //     initializeResult.IsSuccess.Should().Be(true);
    //     var removeResult = await grain.RemoveAsync(new SnackRemoveCommand(Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
    //     removeResult.IsSuccess.Should().Be(true);
    //     var reInitializeResult = await grain.InitializeAsync(new SnackInitializeCommand("Chocolate", Guid.NewGuid(), DateTimeOffset.UtcNow, "Janet"));
    //     reInitializeResult.IsSuccess.Should().Be(false);
    //     _output.WriteLine(reInitializeResult.ToString());
    //     await Task.Delay(1000);
    //     var getResult = await grain.GetNameAsync();
    //     getResult.IsSuccess.Should().Be(true);
    //     await Task.Delay(1000);
    //     _output.WriteLine(getResult.ToString());
    // }
}
