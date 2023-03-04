using Orleans.TestingHost;
using Xunit;

namespace EMachine.Domain.Tests.Fixtures;

public class ClusterFixture : IAsyncLifetime
{
    public ClusterFixture()
    {
        Cluster = new TestClusterBuilder().AddClientBuilderConfigurator<TestClientBuilderConfigurator>()
                                          .AddSiloBuilderConfigurator<TestSiloConfigurator>()
                                          .Build();
    }

    public TestCluster Cluster { get; }

    /// <inheritdoc />
    public Task InitializeAsync()
    {
        return Cluster.DeployAsync();
    }

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        return Cluster.DisposeAsync()
                      .AsTask();
    }
}
