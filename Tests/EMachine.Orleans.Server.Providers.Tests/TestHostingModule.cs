using System.Globalization;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.AspNetCore;
using Fluxera.Extensions.Hosting.Modules.AspNetCore.HttpApi;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.Tests;

[PublicAPI]
[UsedImplicitly]
public class TestHostingModule : ConfigureApplicationModule
{
    public override void Configure(IApplicationInitializationContext context)
    {
        base.Configure(context);
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("zh-CN");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
        if (context.Environment.IsDevelopment())
        {
            context.UseSwaggerUI();
        }
        else
        {
            context.UseHsts();
            context.UseHttpsRedirection();
        }
        context.UseRouting();
        context.UseEndpoints();
        
        var scopeFactory = context.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}
