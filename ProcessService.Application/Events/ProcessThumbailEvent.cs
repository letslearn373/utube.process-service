using MediatR;

namespace ProcessService.Application.Events
{
    public record ProcessThumbnailEvent(string videoId) : INotification;

    public class ProcessThumbnailEventConsumer : NotificationHandler<ProcessThumbnailEvent>
    {
        protected override void Handle(ProcessThumbnailEvent notification)
        {
            throw new NotImplementedException();
        }
    }
}
