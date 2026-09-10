# uWis

[![Downloads](https://img.shields.io/nuget/dt/Umbraco.Community.uWis?color=cc9900)](https://www.nuget.org/packages/Umbraco.Community.uWis/)
[![NuGet](https://img.shields.io/nuget/vpre/Umbraco.Community.uWis?color=0273B3)](https://www.nuget.org/packages/Umbraco.Community.uWis)
[![GitHub license](https://img.shields.io/github/license/skttl/umbraco-uwis?color=8AB803)](../LICENSE)

uWis synchronizes video files from Umbraco with Wistia. When an editor saves a video, uWis uploads the local file to Wistia and stores the Wistia media ID and playback URL on the Umbraco item.

## Requirements

- Umbraco CMS 18 or newer
- A Wistia account with API access
- A Wistia access token with upload, read, and delete permissions
- The hashed ID of the Wistia project that should receive uploads
- A local Umbraco video upload property

## Installation

~~~sh
dotnet add package Umbraco.Community.uWis
~~~

## Wistia configuration

Add the following section to appsettings.json:

~~~json
{
  "Umbraco": {
    "Wistia": {
      "ApiBasePath": "https://api.wistia.com",
      "UploadBasePath": "https://upload.wistia.com",
      "ApiKey": "YOUR_WISTIA_ACCESS_TOKEN",
      "ProjectId": "YOUR_WISTIA_PROJECT_HASHED_ID",
      "ApiVersion": "2026-07"
    }
  }
}
~~~

The access token is sent as a Bearer token and remains server-side. ProjectId is the hashed ID of the Wistia project where new media should be placed.

The same settings can be supplied with environment variables:

~~~text
Umbraco__Wistia__ApiKey=YOUR_WISTIA_ACCESS_TOKEN
Umbraco__Wistia__ProjectId=YOUR_WISTIA_PROJECT_HASHED_ID
Umbraco__Wistia__ApiVersion=2026-07
~~~

Create an access token with upload, read, and delete permissions in Wistia's developer settings.

<!-- Screenshot needed: Wistia developer settings showing token creation and permissions. Do not include the actual token. -->

![Wistia API token](../docs/wistia_api_token.png)

Find the hashed project ID in the Wistia project URL or project settings.

<!-- Screenshot needed: Wistia project settings showing the hashed project ID. -->

![Wistia project ID](../docs/wistia_project.png)

See Wistia's [Upload API documentation](https://docs.wistia.com/reference/post_) and [authentication documentation](https://docs.wistia.com/docs/authentication).

## Add the property editor

uWis adds a Wistia Sync property editor to the Umbraco backoffice.

1. Open Settings > Data Types.
2. Create a data type using the Wistia Sync property editor.
3. In Upload Property Alias, enter the alias of the Upload property containing the video file.
4. Add the new data type to the same media, content, or member type as the Upload property.
5. Save the data type and the content type.

The upload property and the Wistia Sync property must be on the same content type.

<!-- Screenshot needed: Umbraco Data Type editor showing Wistia Sync and Upload Property Alias. -->

![Wistia Sync data type settings](../docs/umbraco_wistia_sync_data_type.png)

## Upload and synchronization

When an editor saves an item, uWis checks whether the configured upload property changed. If it did, uWis:

1. Deletes the previously linked Wistia media, if one exists.
2. Uploads the new file through Wistia's multipart Upload API.
3. Places the media in the configured Wistia project.
4. Stores the Wistia media ID and iframe URL in the sync property.

Wistia processes uploaded media asynchronously. The backoffice editor polls Wistia and shows Preparing, Ready, or Error status.

For a video that existed before the sync property was added, save the item again to trigger synchronization.

<!-- Screenshot needed: Umbraco media or content item showing a Wistia video synchronized successfully. -->

![Synchronized Wistia video in Umbraco](../docs/umbraco_wistia_media.png)

## Stored value

The sync property stores a JSON value converted to uWis.Models.WistiaValue:

~~~json
{
  "Src": "/media/example/video.mp4",
  "WistiaAssetId": "abc123xyz",
  "PlaybackUrl": "https://fast.wistia.net/embed/iframe/abc123xyz"
}
~~~

WistiaAssetId is the Wistia hashed media ID. PlaybackUrl is the Wistia iframe URL.

## Embed a video

~~~cshtml
@if (Model.WistiaVideo?.PlaybackUrl is { } url)
{
    <iframe
        src="@url"
        style="width: 100%; border: none; aspect-ratio: 16/9;"
        allow="autoplay; fullscreen"
        allowfullscreen>
    </iframe>
}
~~~

## Troubleshooting

### Nothing is uploaded

- Check the Wistia access token and its upload permission.
- Confirm that ProjectId is the hashed ID of a real Wistia project.
- Confirm that the Wistia Sync property and upload property are on the same content type.
- Confirm that Upload Property Alias exactly matches the upload property's alias.
- Check the Umbraco logs for the Wistia request error.

### The status stays at Preparing

Wistia processes uploaded media asynchronously. Check the media in Wistia and inspect its processing status.

### The player does not load

- Confirm that the Wistia media ID is valid.
- Check Wistia privacy and embedding settings.
- Confirm that the media has finished processing.

## Contributing

The repository includes a test site for local development. Keep Wistia credentials in local secrets or appsettings.Development.json, never in committed source.

