namespace ProcessService.Application.Service;

public interface IFFMpegService
{
    public Task<(string thumbnailPath, long timeTaken)> GenerateThumbnailAsync(string videoPath, CancellationToken cancellationToken = default);
}
