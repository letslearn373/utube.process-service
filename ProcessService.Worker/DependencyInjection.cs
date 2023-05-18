using MassTransit;
using ProcessService.Worker.Consumers;
using ProcessService.Worker.Protos;
using ProcessService.Worker.Services;
using ProcessService.Worker.Settings;

namespace ProcessService.Worker;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        #region MassTransit

        var rabbitMQSetting = new RabbitMQSetting();
        configuration.GetSection(nameof(RabbitMQSetting)).Bind(rabbitMQSetting);
        services.AddSingleton(rabbitMQSetting);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<VideoUploadedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMQSetting.Endpoint, rabbitMQSetting.VirtualHost, h =>
                {
                    h.Username(rabbitMQSetting.Username);
                    h.Password(rabbitMQSetting.Password);
                });

                cfg.ConfigureEndpoints(context);
            });

        });

        #endregion

        #region gRPC

        var grpcFileServiceHost = configuration.GetSection("GrpcClients:StorageService").Value;

        if (!string.IsNullOrEmpty(grpcFileServiceHost))
        {
            services.AddGrpcClient<GrpcFileService.GrpcFileServiceClient>(option =>
            {
                option.Address = new Uri(grpcFileServiceHost);
            });
        }

        #endregion

        services.Configure<FFMpegSetting>(configuration.GetSection(nameof(FFMpegSetting)));

        services.AddTransient<IFFMpegService, FFMpegService>();
        services.AddTransient<IFileService, FileService>();



        return services;
    }
}
