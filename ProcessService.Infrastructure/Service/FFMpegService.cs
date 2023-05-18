using Microsoft.Extensions.Options;
using ProcessService.Application.Service;
using ProcessService.Infrastructure.Settings;
using System.Diagnostics;

namespace ProcessService.Infrastructure.Service;

public class FFMpegService : IFFMpegService
{
    private readonly FFMpegSetting _ffMpegSetting;

    public FFMpegService(IOptions<FFMpegSetting> option)
    {
        _ffMpegSetting = option.Value;
    }

    public async Task<(string thumbnailPath, long timeTaken)> GenerateThumbnailAsync(string videoPath, CancellationToken cancellationToken = default)
    {
        var outputPath = GetFilePathFromTempDirectory($"{Guid.NewGuid()}.png");
        var arguments = $@"-i {videoPath} -vf ""thumbnail=300"" -frames:v 1 {outputPath}";
        var timeTaken = await ProcessVideoAsync(arguments);
        return await Task.FromResult((outputPath, timeTaken));
    }

    private string GetFilePathFromTempDirectory(string filename)
    {
        return Path.Combine(Path.GetTempPath(), filename);
    }

    private Task<long> ProcessVideoAsync(string arguments)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var tcs = new TaskCompletionSource<long>();

        var process = new Process
        {
            StartInfo =
            {
                FileName = _ffMpegSetting.FFMpegPath,
                Arguments = arguments
            },
            EnableRaisingEvents = true,
        };

        process.Exited += (sender, args) =>
        {
            process.Dispose();
            stopwatch.Stop();
            tcs.SetResult(stopwatch.ElapsedMilliseconds);
        };

        process.Start();
        return tcs.Task;
    }
}
