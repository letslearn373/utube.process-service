using MassTransit;
using MediatR;
using UTube.Common.Events;
using ProcessService.Application.Events;

namespace ProcessService.Application.Consumers
{
    public class VideoUploadedEventConsumer : IConsumer<VideoUploadedEvent>
    {
        private readonly IPublisher _publisher;

        public VideoUploadedEventConsumer(IPublisher mediator)
        {
            _publisher = mediator;
        }

        public async Task Consume(ConsumeContext<VideoUploadedEvent> context)
        {
            await _publisher.Publish(new ProcessThumbnailEvent(context.Message.videoId, context.Message.objectUrl), context.CancellationToken).ConfigureAwait(false);
            await _publisher.Publish(new ResizeVideoEvent(context.Message.objectUrl));
        }
    }
}
