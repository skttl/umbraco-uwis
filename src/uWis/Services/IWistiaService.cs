using System.Text.Json.Serialization;

namespace uWis.Services;

public interface IWistiaService
{
    Task<WistiaAsset?> GetAsset(string assetId, CancellationToken cancellationToken = default);
    Task<WistiaAsset> CreateAsset(byte[] bytes, string? title = null, string? creatorId = null, string? externalId = null, CancellationToken cancellationToken = default);
    Task DeleteAsset(string assetId, CancellationToken cancellationToken = default);
}

public sealed class WistiaAsset
{
    [JsonPropertyName("asset_id")]
    public string? AssetId { get; set; }
    public string? Status { get; set; }
    public WistiaOutput? Output { get; set; }
    [JsonPropertyName("upload_url")]
    public string? UploadUrl { get; set; }
}

public sealed class WistiaOutput
{
    [JsonPropertyName("status_url")] public string? StatusUrl { get; set; }
    [JsonPropertyName("playback_url")] public string? PlaybackUrl { get; set; }
}
