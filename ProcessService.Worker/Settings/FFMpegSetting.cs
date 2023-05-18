namespace ProcessService.Worker.Settings;

public class FFMpegSetting
{
    public string Directory { get; set; } = string.Empty;
    public string FFMpegFileName { get; set; } = string.Empty;
    public string FFMpegPath => Path.Combine(Directory, FFMpegFileName);
}
