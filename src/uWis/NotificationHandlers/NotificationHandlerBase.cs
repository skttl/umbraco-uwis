using System.Text.Json;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using uWis.Models;
using uWis.Services;

namespace uWis.NotificationHandlers;

public abstract class NotificationHandlerBase
{
    private readonly ILogger<NotificationHandlerBase> _logger;
    private readonly IDataTypeService _dataTypeService;
    private readonly IWistiaService _wistiaService;
    private readonly MediaFileManager _mediaFileManager;

    public NotificationHandlerBase(
        ILogger<NotificationHandlerBase> logger,
        IDataTypeService dataTypeService,
        IWistiaService wistiaService,
        MediaFileManager mediaFileManager)
    {
        _logger = logger;
        _dataTypeService = dataTypeService;
        _wistiaService = wistiaService;
        _mediaFileManager = mediaFileManager;
    }

    public async Task<bool> TryDeleteSyncedUploadFilesFromWistia(IContentBase node)
    {
        try
        {
            return await DeleteSyncedUploadFilesFromWistia(node);
        }
        catch
        {
            _logger.LogError(
                "An error occurred while deleting synced upload files from Wistia for content with ID {ContentId}",
                node.Id
            );
            return false;
        }
    }

    public async Task<bool> TrySyncUploadFilesToWistia(IContentBase node)
    {
        try
        {
            return await SyncUploadFilesToWistia(node);
        }
        catch
        {
            _logger.LogError(
                "An error occurred while syncing upload files to Wistia for content with ID {ContentId}",
                node.Id
            );
            return false;
        }
    }

    /// <summary>
    /// Syncs upload files to Wistia when the content has uWis sync properties.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>boolean indicating if any changes were made</returns>
    public async Task<bool> SyncUploadFilesToWistia(IContentBase node)
    {
        var isUpdated = false;

        var wistiaSyncProperties = node.Properties.Where(x =>
            x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorSchema
        );

        foreach (var wistiaSyncProperty in wistiaSyncProperties)
        {
            var dataType = await _dataTypeService.GetAsync(
                wistiaSyncProperty.PropertyType.DataTypeKey
            );

            if (
                dataType is null
                || dataType.ConfigurationData.TryGetValue(
                    Constants.UploadPropertyAlias,
                    out var value
                )
                    is false
                || value is not string uploadPropertyAlias
                || string.IsNullOrWhiteSpace(uploadPropertyAlias)
            )
            {
                continue;
            }

            var existingStringValue = node.GetValue<string>(wistiaSyncProperty.Alias);

            var existingValue =
                existingStringValue.IsNullOrWhiteSpace() is false
                && existingStringValue.StartsWith("{")
                    ? JsonSerializer.Deserialize<WistiaValue>(existingStringValue)
                    : null;

            var canContinue =
                node.IsPropertyDirty(uploadPropertyAlias)
                || existingValue?.Src != node.GetValue<string>(uploadPropertyAlias);

            if (canContinue == false)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(existingValue?.WistiaAssetId) is false)
            {
                await _wistiaService.DeleteAsset(existingValue.WistiaAssetId);
                node.SetValue(wistiaSyncProperty.Alias, null);
                isUpdated = true;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                var fileStream = _mediaFileManager.GetFile(node, out var _, uploadPropertyAlias);
                fileStream.CopyTo(ms);
                var byteArray = ms.ToArray();

                if (byteArray is not null && byteArray.Length > 0)
                {
                    var asset = await _wistiaService.CreateAsset(
                        byteArray,
                        node.Name,
                        node.CreatorId.ToString(),
                        node.GetUdi().ToString().EnsureEndsWith($"/{uploadPropertyAlias}")
                    );

                    node.SetValue(
                        wistiaSyncProperty.Alias,
                        JsonSerializer.Serialize(
                            new WistiaValue()
                            {
                                WistiaAssetId = asset.AssetId,
                                PlaybackUrl = asset.Output?.PlaybackUrl,
                                Src = node.GetValue<string>(uploadPropertyAlias),
                            }
                        )
                    );

                    isUpdated = true;
                }
            }
        }

        return isUpdated;
    }

    /// <summary>
    /// Deletes the files from Wistia if the content has any uWis sync properties.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>boolean indicating if any changes were made</returns>
    public async Task<bool> DeleteSyncedUploadFilesFromWistia(IContentBase node)
    {
        var isUpdated = false;

        var wistiaSyncProperties = node.Properties.Where(x =>
            x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorSchema
        );

        foreach (var wistiaSyncProperty in wistiaSyncProperties)
        {
            var dataType = await _dataTypeService.GetAsync(
                wistiaSyncProperty.PropertyType.DataTypeKey
            );

            var existingStringValue = node.GetValue<string>(wistiaSyncProperty.Alias);

            var existingValue =
                existingStringValue.IsNullOrWhiteSpace() is false
                && existingStringValue.StartsWith("{")
                    ? JsonSerializer.Deserialize<WistiaValue>(existingStringValue)
                    : null;

            if (existingValue is null || existingValue.WistiaAssetId.IsNullOrWhiteSpace())
            {
                continue;
            }

            await _wistiaService.DeleteAsset(existingValue.WistiaAssetId);
            node.SetValue(wistiaSyncProperty.Alias, null);
            isUpdated = true;
        }

        return isUpdated;
    }

    public static bool ResetWistiaValuesWithoutDeleting(IContentBase node)
    {
        var isUpdated = false;

        var wistiaSyncProperties = node.Properties.Where(x =>
            x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorSchema
        );

        foreach (var wistiaSyncProperty in wistiaSyncProperties)
        {
            node.SetValue(wistiaSyncProperty.Alias, null);
            isUpdated = isUpdated || node.IsPropertyDirty(wistiaSyncProperty.Alias);
        }

        return isUpdated;
    }
}
