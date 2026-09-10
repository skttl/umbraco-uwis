using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using uWis.Services;

namespace uWis.NotificationHandlers;

public class ContentNotifications
    : NotificationHandlerBase,
        INotificationHandler<ContentCopyingNotification>,
        INotificationAsyncHandler<ContentDeletedBlueprintNotification>,
        INotificationAsyncHandler<ContentDeletedNotification>,
        INotificationHandler<ContentSavedBlueprintNotification>,
        INotificationAsyncHandler<ContentSavingNotification>
{
    private readonly IContentService _contentService;

    public ContentNotifications(
        ILogger<ContentNotifications> logger,
        IContentService contentService,
        IDataTypeService dataTypeService,
        IWistiaService wistiaService,
        MediaFileManager mediaFileManager
    )
        : base(logger, dataTypeService, wistiaService, mediaFileManager)
    {
        _contentService = contentService;
    }

    public void Handle(ContentCopyingNotification notification) =>
        ResetWistiaValuesWithoutDeleting(notification.Copy);

    public async Task HandleAsync(
        ContentDeletedBlueprintNotification notification,
        CancellationToken cancellationToken
    ) => await Task.WhenAll(notification.DeletedBlueprints.Select(TryDeleteSyncedUploadFilesFromWistia));

    public async Task HandleAsync(
        ContentDeletedNotification notification,
        CancellationToken cancellationToken
    ) => await Task.WhenAll(notification.DeletedEntities.Select(TryDeleteSyncedUploadFilesFromWistia));

    public void Handle(ContentSavedBlueprintNotification notification)
    {
        if (ResetWistiaValuesWithoutDeleting(notification.SavedBlueprint))
        {
            _contentService.SaveBlueprint(
                notification.SavedBlueprint,
                notification.CreatedFromContent
            );
        }
    }

    public async Task HandleAsync(
        ContentSavingNotification notification,
        CancellationToken cancellationToken
    ) => await Task.WhenAll(notification.SavedEntities.Select(TrySyncUploadFilesToWistia));
}
