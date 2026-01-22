using OrchardCore.Logging;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, configBuilder) =>
{
    configBuilder.ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.OpenTelemetry(options =>
        {
            // Aspire provides the OTLP endpoint in the configuration
            options.Endpoint = hostingContext.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
            options.Protocol = OtlpProtocol.Grpc;
            // Add resource attributes to identify the service in the dashboard
            options.ResourceAttributes.Add("service.name", builder.Environment.ApplicationName);
        });
});

var orchardCmsBuilder = builder.Services
    .AddOrchardCms()
    .AddSetupFeatures("OrchardCore.AutoSetup");

builder.AddServiceDefaults();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseOrchardCore(c => c.UseSerilogTenantNameLogging());

app.Run();
