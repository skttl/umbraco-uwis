using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using uWis.Services;

namespace uWis.NotificationHandlers;

public class MediaNotifications
    : NotificationHandlerBase,
        INotificationAsyncHandler<MediaSavingNotification>,
        INotificationAsyncHandler<MediaDeletedNotification>
{
    public MediaNotifications(
        ILogger<MediaNotifications> logger,
        IDataTypeService dataTypeService,
        IWistiaService wistiaService,
        MediaFileManager mediaFileManager
    )
        : base(logger, dataTypeService, wistiaService, mediaFileManager) { }

    public async Task HandleAsync(
        MediaSavingNotification notification,
        CancellationToken cancellationToken
    ) => await Task.WhenAll(notification.SavedEntities.Select(TrySyncUploadFilesToWistia));

    public async Task HandleAsync(
        MediaDeletedNotification notification,
        CancellationToken cancellationToken
    ) => await Task.WhenAll(notification.DeletedEntities.Select(TryDeleteSyncedUploadFilesFromWistia));
}
