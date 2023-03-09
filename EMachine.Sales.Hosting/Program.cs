using Fluxera.Extensions.Hosting;

namespace EMachine.Sales.Hosting;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        await ApplicationHost.RunAsync<SalesApplicationHost>(args);
    }
}
