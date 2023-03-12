using EMachine.Orleans.Server.Providers.Tests;
using Fluxera.Extensions.Hosting;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        await ApplicationHost.RunAsync<TestApplicationHost>(args);
    }
}
