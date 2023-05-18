using MediatR;
using MimeTypes;
using ProcessService.Application.Protos;
using ProcessService.Application.Service;
using static ProcessService.Application.Protos.GrpcFileService;

namespace ProcessService.Application.Events
{
    public record ProcessThumbnailEvent(string videoId, string videoPath) : INotification;

    public class ProcessThumbnailEventConsumer : INotificationHandler<ProcessThumbnailEvent>
    {
        private readonly GrpcFileServiceClient _grpcFileServiceClient;
        private readonly IFFMpegService _ffMpegService;
        private readonly IFileService _fileService;

        public ProcessThumbnailEventConsumer(GrpcFileServiceClient grpcFileServiceClient,
            IFFMpegService ffMpegService,
            IFileService fileService)
        {
            _grpcFileServiceClient = grpcFileServiceClient;
            _ffMpegService = ffMpegService;
            _fileService = fileService;
        }

        public async Task Handle(ProcessThumbnailEvent notification, CancellationToken cancellationToken)
        {
            var (thumbnailPath, timetaken) = await _ffMpegService.GenerateThumbnailAsync(notification.videoPath);

            if (!string.IsNullOrWhiteSpace(thumbnailPath) && File.Exists(thumbnailPath))
            {
                var uploadStream = _grpcFileServiceClient.UploadFile();

                await _fileService.PrepareFileStreamAsync(thumbnailPath, (data) =>
                {
                    uploadStream.RequestStream.WriteAsync(new UploadFileRequest()
                    {
                        Chunk = new DataChunk { Data = data },
                        VideoId = notification.videoId,
                        MimeType = MimeTypeMap.GetMimeType(thumbnailPath),
                        Type = UploadFileType.Thumbnail
                    }, cancellationToken).Wait(cancellationToken);
                }, cancellationToken);

                await uploadStream.RequestStream.CompleteAsync();

                File.Delete(thumbnailPath);
            }

            await Task.CompletedTask;
        }
    }
}
