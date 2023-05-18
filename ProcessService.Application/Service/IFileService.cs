using Google.Protobuf;

namespace ProcessService.Application.Service
{
    public interface IFileService
    {
        public Task PrepareFileStreamAsync(string filePath, Action<ByteString> action, CancellationToken cancellationToken = default);
    }
}
