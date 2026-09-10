const t = {
  name: "uWis Entry Point",
  type: "backofficeEntryPoint",
  alias: "uWis.EntryPoint",
  js: () => import("./entrypoint-Ds0YLQXJ.js")
}, e = {
  type: "propertyEditorUi",
  alias: "uWis.PropertyEditorUi.WistiaSync",
  name: "Wistia Sync",
  element: () => import("./property-editor-ui-wistia-sync.element-DVj12vzL.js"),
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
}, i = {
  type: "propertyEditorSchema",
  name: "Wistia Sync",
  alias: "uWis.Sync",
  meta: {
    defaultPropertyEditorUiAlias: "uWis.PropertyEditorUi.WistiaSync"
  }
}, o = [
  t,
  e,
  i
];
export {
  o as manifests
};
//# sourceMappingURL=u-gum.js.map
