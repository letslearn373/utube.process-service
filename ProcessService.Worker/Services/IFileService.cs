using Google.Protobuf;

namespace ProcessService.Worker.Services
{
    public interface IFileService
    {
        public Task PrepareFileStreamAsync(string filePath, Action<ByteString> action, CancellationToken cancellationToken = default);
    }
}
