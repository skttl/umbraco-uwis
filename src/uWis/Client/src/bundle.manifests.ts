import { ManifestPropertyEditorSchema, ManifestPropertyEditorUi } from '@umbraco-cms/backoffice/property-editor'

const entryPointManifest = {
  name: 'uWis Entry Point',
  type: 'backofficeEntryPoint',
  alias: 'uWis.EntryPoint',
  js: () => import('./entrypoint.js'),
}

const editorManifest: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'uWis.PropertyEditorUi.WistiaSync',
  name: 'Wistia Sync',
  element: () => import('./property-editor-ui-wistia-sync.element.js'),
  meta: {
    label: 'Wistia Sync',
    icon: 'icon-video',
    group: 'media',
    propertyEditorSchemaAlias: 'uWis.Sync',

    settings: {
      properties: [
        {
          alias: "uploadPropertyAlias",
          label: "Upload Property Alias",
          description: "Set the alias of the upload property editor to sync with",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox",
        },
      ],
    },
  },
};

const schemaManifest: ManifestPropertyEditorSchema = {
  type: 'propertyEditorSchema',
  name: 'Wistia Sync',
  alias: 'uWis.Sync',
  meta: {
    defaultPropertyEditorUiAlias: 'uWis.PropertyEditorUi.WistiaSync'
  },
};

export const manifests = [
  entryPointManifest,
  editorManifest,
  schemaManifest,
];
