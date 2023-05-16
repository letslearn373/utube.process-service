using MediatR;

namespace ProcessService.Application.Events
{
    public record ResizeVideoEvent(string videoId) : INotification;

    public class ResizeVideoEventConsumer : NotificationHandler<ProcessThumbnailEvent>
    {
        protected override void Handle(ProcessThumbnailEvent notification)
        {
            throw new NotImplementedException();
        }
    }
}
