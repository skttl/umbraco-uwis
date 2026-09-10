using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using uWis.Configuration;

namespace uWis.Services;

public sealed class WistiaService : IWistiaService
{
    private readonly HttpClient _apiClient;
    private readonly HttpClient _uploadClient;
    private readonly WistiaSettings _settings;

    public WistiaService(IOptionsMonitor<WistiaSettings> options, IHttpClientFactory httpClientFactory)
    {
        _settings = options.CurrentValue;
        _apiClient = httpClientFactory.CreateClient(nameof(WistiaService));
        _apiClient.BaseAddress = new Uri(_settings.ApiBasePath.TrimEnd('/') + "/");
        _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        _apiClient.DefaultRequestHeaders.Add("X-Wistia-API-Version", _settings.ApiVersion);

        _uploadClient = httpClientFactory.CreateClient("WistiaUpload");
        _uploadClient.BaseAddress = new Uri(_settings.UploadBasePath.TrimEnd('/') + "/");
        _uploadClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task<WistiaAsset?> GetAsset(string assetId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetId);
        using var response = await _apiClient.GetAsync($"modern/medias/{Uri.EscapeDataString(assetId)}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var media = await response.Content.ReadFromJsonAsync<WistiaMediaResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Wistia returned an empty media response.");
        return Map(media);
    }

    public async Task<WistiaAsset> CreateAsset(byte[] bytes, string? title = null, string? creatorId = null, string? externalId = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_settings.ApiKey);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bytes.Length);

        using var form = new MultipartFormDataContent();
        form.Add(new ByteArrayContent(bytes), "file", title ?? "umbraco-video.mp4");
        if (!string.IsNullOrWhiteSpace(title)) form.Add(new StringContent(title), "name");
        if (!string.IsNullOrWhiteSpace(_settings.ProjectId)) form.Add(new StringContent(_settings.ProjectId), "project_id");
        if (!string.IsNullOrWhiteSpace(creatorId)) form.Add(new StringContent(creatorId), "description");

        using var response = await _uploadClient.PostAsync(string.Empty, form, cancellationToken);
        response.EnsureSuccessStatusCode();
        var media = await response.Content.ReadFromJsonAsync<WistiaMediaResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Wistia returned an empty upload response.");
        ArgumentException.ThrowIfNullOrWhiteSpace(media.HashedId);
        return Map(media);
    }

    public async Task DeleteAsset(string assetId, CancellationToken cancellationToken = default)
    {
        using var response = await _apiClient.DeleteAsync($"v1/medias/{Uri.EscapeDataString(assetId)}.json", cancellationToken);
        if (response.StatusCode != HttpStatusCode.NotFound) response.EnsureSuccessStatusCode();
    }

    private static WistiaAsset Map(WistiaMediaResponse media)
    {
        var id = media.HashedId;
        return new WistiaAsset
        {
            AssetId = id,
            Status = media.Status,
            Output = new WistiaOutput
            {
                StatusUrl = id is null ? null : $"/modern/medias/{id}",
                PlaybackUrl = id is null ? null : $"https://fast.wistia.net/embed/iframe/{id}"
            }
        };
    }
}
