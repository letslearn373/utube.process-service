using UTube.ProcessService.Application;
using UTube.ProcessService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication(builder.Configuration);
};

var app = builder.Build();
{
    app.MapGet("/", () => "Hello World!");
    app.Run();
}
