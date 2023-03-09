using System.Globalization;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.AspNetCore;
using Fluxera.Extensions.Hosting.Modules.AspNetCore.HttpApi;
using JetBrains.Annotations;

namespace EMachine.Sales.Hosting;

[PublicAPI]
[UsedImplicitly]
public class SalesHostingModule : ConfigureApplicationModule
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
    }
}
