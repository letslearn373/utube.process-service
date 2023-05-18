using ProcessService.Worker;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddDependencies(builder.Configuration);
};

var app = builder.Build();
{
    app.MapGet("/", () => "Hello World!");
    app.Run();
}
