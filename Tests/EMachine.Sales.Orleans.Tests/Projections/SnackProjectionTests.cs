using EMachine.Sales.Orleans.Tests.Fixtures;
using FluentAssertions;
using Fluxera.Extensions.Common;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests.Projections;

[Collection(SnackProjectionCollectionFixture.Name)]
public class SnackProjectionTests
{
    private readonly ITestOutputHelper _output;
    private readonly TestCluster _cluster;

    public SnackProjectionTests(ClusterFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _cluster = fixture.Cluster;
    }

    [Fact]
    public void Should_Get_IdGenerator()
    {
        var guidGenerator = _cluster.ServiceProvider.GetRequiredService<IGuidGenerator>();
        guidGenerator.Should().NotBeNull();
        _output.WriteLine(guidGenerator.Create().ToString("D"));
        _output.WriteLine(guidGenerator.Create().ToString("D"));
        _output.WriteLine(guidGenerator.Create().ToString("D"));
        _output.WriteLine(guidGenerator.Create().ToString("D"));
        _output.WriteLine(guidGenerator.Create().ToString("D"));
    }
}
