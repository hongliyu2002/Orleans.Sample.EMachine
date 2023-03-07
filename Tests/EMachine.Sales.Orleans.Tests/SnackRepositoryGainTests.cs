using EMachine.Sales.Orleans.Abstractions;
using EMachine.Sales.Orleans.Abstractions.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(SnackRepositoryCollectionFixture.Name)]
public class SnackRepositoryGainTests : IClassFixture<SnackRepositoryFixture>
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
        var grain = _cluster.GrainFactory.GetGrain<ISnackWriterGrain>(Guid.Empty);
        var createResult = await grain.CreateAsync(new SnackWriterCreateOneCommand(1000, "Apple", Guid.NewGuid(), "Leo"));
        createResult.IsSuccess.Should().Be(true);
        createResult.Value.GetPrimaryKeyLong().Should().Be(1000);
        _testOutputHelper.WriteLine(createResult.ToString());
        var result = await createResult.Value.GetNameAsync();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().Be("Apple");
        _testOutputHelper.WriteLine(createResult.ToString());
    }

    [Fact]
    public async Task Can_Delete_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackWriterGrain>(Guid.Empty);
        var createResult = await grain.CreateAsync(new SnackWriterCreateOneCommand(1001, "Lemon", Guid.NewGuid(), "Leo"));
        createResult.IsSuccess.Should().Be(true);
        createResult.Value.GetPrimaryKeyLong().Should().Be(1001);
        _testOutputHelper.WriteLine(createResult.ToString());
        var deleteResult = await grain.DeleteAsync(new SnackWriterDeleteOneCommand(1001, Guid.NewGuid(), "Boss"));
        deleteResult.IsSuccess.Should().Be(true);
        _testOutputHelper.WriteLine(deleteResult.ToString());
    }

    [Fact]
    public async Task Can_Get_Snack()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackWriterGrain>(Guid.Empty);
        var getResult = await grain.GetAsync(new SnackWriterGetOneCommand(1, Guid.NewGuid(), "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GetPrimaryKeyLong().Should().Be(1);
        var result = await getResult.Value.GetNameAsync();
        result.Value.Should().Be("Cafe");
    }

    [Fact]
    public async Task Can_Get_Snacks()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackWriterGrain>(Guid.Empty);
        var getResult = await grain.GetMultipleAsync(new SnackWriterGetMultipleCommand(new long[]
                                                                                       {
                                                                                           2,
                                                                                           3,
                                                                                           4
                                                                                       }, Guid.NewGuid(), "Boss"));
        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Count.Should().Be(3);
        getResult.Value.ForEach(x => _testOutputHelper.WriteLine(x.ToString()));
    }
}
