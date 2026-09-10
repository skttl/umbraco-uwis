using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using uWis.Enums;
using uWis.Services;

namespace uWis.Controllers;

[ApiVersion(Constants.Swagger.Version)]
[ApiExplorerSettings(GroupName = Constants.Swagger.GroupName)]
public class WistiaApiController : WistiaApiControllerBase
{
    private readonly IWistiaService _wistiaService;

    public WistiaApiController(IWistiaService wistiaService) => _wistiaService = wistiaService;

    [HttpGet("status")]
    [ProducesResponseType<AssetStatus>(StatusCodes.Status200OK)]
    public async Task<AssetStatus> GetStatus(string assetId)
    {
        var asset = await _wistiaService.GetAsset(assetId);
        if (asset == null)
        {
            return AssetStatus.NotFound;
        }

        return asset.Status?.ToLowerInvariant() switch
        {
            "created" or "pre-queued" or "upload-pending" or "uploaded" or "queued" or
            "downloading" or "downloaded" or "processing" or "processed" or
            "stream-ready" or "repackaging" => AssetStatus.Preparing,
            "errored" or "error" => AssetStatus.Errored,
            "ready" => AssetStatus.Ready,
            _ => AssetStatus.Unknown,
        };
    }
}
