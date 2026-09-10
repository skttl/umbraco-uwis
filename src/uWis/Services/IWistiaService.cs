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
    public string? AssetId { get; init; }
    public string? Status { get; init; }
    public WistiaOutput? Output { get; init; }
}

public sealed class WistiaOutput
{
    public string? StatusUrl { get; init; }
    public string? PlaybackUrl { get; init; }
}

internal sealed class WistiaMediaResponse
{
    [JsonPropertyName("hashed_id")] public string? HashedId { get; init; }
    [JsonPropertyName("status")] public string? Status { get; init; }
}
