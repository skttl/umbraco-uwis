namespace uWis.Configuration;

public class WistiaSettings
{
    public string ApiBasePath { get; set; } = "https://api.wistia.com";
    public string UploadBasePath { get; set; } = "https://upload.wistia.com";
    public string? ApiKey { get; set; }
    public string? ProjectId { get; set; }
    public string ApiVersion { get; set; } = "2026-07";
}
