using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcessService.Application.Consumers;
using ProcessService.Application.Service;
using ProcessService.Infrastructure.Service;
using ProcessService.Infrastructure.Settings;

namespace ProcessService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
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

        services.Configure<FFMpegSetting>(configuration.GetSection(nameof(FFMpegSetting)));

        services.AddTransient<IFFMpegService, FFMpegService>();
        services.AddTransient<IFileService, FileService>();

        return services;
    }
}
