using Google.Protobuf;
using ProcessService.Application.Service;

namespace ProcessService.Infrastructure.Service
{
    public class FileService : IFileService
    {
        public async Task PrepareFileStreamAsync(string filePath, Action<ByteString> action, CancellationToken cancellationToken = default)
        {
            if (action == null) return;

            if (File.Exists(filePath))
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    var buffer = new byte[4096];
                    var bytesRead = 0;

                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        if (bytesRead > 0)
                        {
                            var data = ByteString.CopyFrom(buffer, 0, bytesRead);
                            action.Invoke(data);
                        }
                    }
                }
            }
        }
    }
}
