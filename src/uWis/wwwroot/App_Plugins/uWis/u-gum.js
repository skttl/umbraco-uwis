const i = {
  name: "uWis Entry Point",
  type: "backofficeEntryPoint",
  alias: "uWis.EntryPoint",
  js: () => import("./entrypoint-Ds0YLQXJ.js")
}, t = {
  type: "propertyEditorUi",
  alias: "uWis.PropertyEditorUi.WistiaSync",
  name: "Wistia Sync",
  element: () => import("./property-editor-ui-wistia-sync.element-D4S3Ysrs.js"),
  meta: {
    label: "Wistia Sync",
    icon: "icon-video",
    group: "media",
    propertyEditorSchemaAlias: "uWis.Sync",
    settings: {
      properties: [
        {
          alias: "uploadPropertyAlias",
          label: "Upload Property Alias",
          description: "Set the alias of the upload property editor to sync with",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox"
        }
      ]
    }
  }
}, e = {
  type: "propertyEditorSchema",
  name: "Wistia Sync",
  alias: "uWis.Sync",
  meta: {
    defaultPropertyEditorUiAlias: "uWis.PropertyEditorUi.WistiaSync"
  }
}, o = [
  i,
  t,
  e
];
export {
  o as manifests
};
//# sourceMappingURL=u-gum.js.map
