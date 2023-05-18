using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcessService.Application.Protos;

namespace ProcessService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        var grpcFileServiceHost = configuration.GetSection("GrpcClients:StorageService").Value;

        if (!string.IsNullOrEmpty(grpcFileServiceHost))
        {
            services.AddGrpcClient<GrpcFileService.GrpcFileServiceClient>(option =>
            {
                option.Address = new Uri(grpcFileServiceHost);
            });
        }

        return services;
    }
}
