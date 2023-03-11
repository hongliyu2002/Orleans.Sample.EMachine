using System.Collections.Immutable;
using EMachine.Sales.Orleans.Queries;
using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests;

[Collection(TestCollectionFixture.Name)]
public class SnackQueryRepoGrainTests : IClassFixture<SnackQueryRepoFixture>
{
    private readonly TestCluster _cluster;
    private readonly ITestOutputHelper _output;

    public SnackQueryRepoGrainTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _cluster = fixture.Cluster;
        _output = output;
    }

    [Fact]
    public async Task Can_List_All_Snacks()
    {
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackQueryRepoGrain>(Guid.Empty);
        var listPagedResult = await repoGrain.ListPagedAsync(new SnackPagedListQuery(0, 100, ImmutableArray<KeyValuePair<string, bool>>.Empty.Add(new("Name", false)), Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        listPagedResult.IsSuccess.Should().BeTrue();
        listPagedResult.Value.Count.Should().Be(12);
        listPagedResult.Value.ForEach(view => _output.WriteLine(view.ToString()));
    }
    
    [Fact]
    public async Task Can_List_2nd_Page_Snacks()
    {
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackQueryRepoGrain>(Guid.Empty);
        var listPagedResult = await repoGrain.ListPagedAsync(new SnackPagedListQuery(5, 5, ImmutableArray<KeyValuePair<string, bool>>.Empty.Add(new("Name", false)), Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        listPagedResult.IsSuccess.Should().BeTrue();
        listPagedResult.Value.Count.Should().Be(5);
        listPagedResult.Value.ForEach(view => _output.WriteLine(view.ToString()));
    }
    
    [Fact]
    public async Task Can_Search_Snacks()
    {
        var repoGrain = _cluster.GrainFactory.GetGrain<ISnackQueryRepoGrain>(Guid.Empty);
        var searchPagedResult = await repoGrain.SearchPagedAsync(new SnackSearchPagedListQuery("er", 0, 100, ImmutableArray<KeyValuePair<string, bool>>.Empty.Add(new("Name", false)), Guid.NewGuid(), DateTimeOffset.UtcNow, "Boss"));
        searchPagedResult.IsSuccess.Should().BeTrue();
        searchPagedResult.Value.Count.Should().Be(3);
        searchPagedResult.Value.ForEach(view => _output.WriteLine(view.ToString()));
    }
}
