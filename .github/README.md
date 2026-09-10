# uWis

uWis synchronizes Umbraco video files with Wistia.

## Installation

`sh
dotnet add package Umbraco.Community.uWis
`

Add a Wistia Sync property to the same type as the source video property and set Upload Property Alias to the source property alias.

## Configuration

Create a Wistia access token with upload, read, and delete permissions. ProjectId is the hashed ID of the target Wistia project.

`json
{
  "Umbraco": {
    "Wistia": {
      "ApiKey": "YOUR_WISTIA_ACCESS_TOKEN",
      "ProjectId": "YOUR_WISTIA_PROJECT_HASHED_ID",
      "ApiVersion": "2026-07"
    }
  }
}
`

The token is sent as a Bearer token and remains server-side.

## API behavior

uWis uploads with POST https://upload.wistia.com/ as multipart form data, reads status with GET /modern/medias/{hashed_id}, and deletes with DELETE /v1/medias/{hashed_id}.json.

The stored playback value is the Wistia iframe URL. The stored value contains Src, WistiaAssetId, and PlaybackUrl.

## References

- [Wistia Upload API](https://docs.wistia.com/reference/post_)
- [Wistia Show Media](https://docs.wistia.com/reference/get_medias-mediahashedid)
- [Wistia authentication](https://docs.wistia.com/docs/authentication)

