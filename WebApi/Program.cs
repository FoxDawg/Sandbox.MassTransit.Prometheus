using MassTransit;
using Prometheus;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer(typeof(HeartBeatConsumer));
    busConfigurator.UsingInMemory((context, configurator) => configurator.UsePrometheusMetrics(serviceName: "my-service"));
});
builder.Services.AddHostedService<HeartBeatSender>();

var app = builder.Build();

app.UseRouting();
app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoints => endpoints.MapMetrics());
app.Run();