using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using ContractCreator.ContentTypes.Drivers;
using ContractCreator.ContentTypes.Handlers;
using ContractCreator.ContentTypes.Models;
using ContractCreator.ContentTypes.Settings;
using ContractCreator.ContentTypes.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using OrchardCore.Redis;

namespace ContractCreator.ContentTypes;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TemplateOptions>(o =>
        {
            o.MemberAccessStrategy.Register<TestPartViewModel>();
        });

        services.AddContentPart<TestPart>()
            .UseDisplayDriver<TestPartDisplayDriver>()
            .AddHandler<TestPartHandler>();

        services.AddScoped<IContentTypePartDefinitionDisplayDriver, TestPartSettingsDisplayDriver>();
        services.AddDataMigration<Migrations>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        routes.MapAreaControllerRoute(
            name: "Home",
            areaName: "ContractCreator.ContentTypes",
            pattern: "Home/Index",
            defaults: new { controller = "Home", action = "Index" }
        );
    }
}

[Feature("OrchardCore.Redis.DataProtection-Fixed")]
public sealed class RedisDataProtectionStartup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        if (services.Any(d => d.ServiceType == typeof(IRedisService)))
        {
            // Remove any previously registered options setups.
            services.RemoveAll<IConfigureOptions<KeyManagementOptions>>();

            services.AddTransient<IConfigureOptions<KeyManagementOptions>, RedisKeyManagementOptionsSetup2>();
        }
    }
}

