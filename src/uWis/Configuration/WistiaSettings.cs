namespace uWis.Configuration;

public class WistiaSettings
{
    public string ApiBasePath { get; set; } = "https://api.wistia.com";
    public string? ApiKey { get; set; }
    public string? SourceId { get; set; }
    public string Format { get; set; } = "hls";
    public string[] Resolution { get; set; } = ["240p", "360p", "480p", "720p", "1080p"];
    public bool KeepOriginal { get; set; } = false;
}
