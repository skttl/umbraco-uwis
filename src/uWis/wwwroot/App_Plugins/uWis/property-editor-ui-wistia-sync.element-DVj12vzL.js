import { html as c, property as E, state as m, customElement as R } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement as f } from "@umbraco-cms/backoffice/lit-element";
import { c as y } from "./client.gen-Crtm1jAh.js";
var s = /* @__PURE__ */ ((t) => (t.NOT_FOUND = "NotFound", t.UNKNOWN = "Unknown", t.PREPARING = "Preparing", t.ERRORED = "Errored", t.READY = "Ready", t))(s || {});
class N {
  static getStatus(e) {
    return ((e == null ? void 0 : e.client) ?? y).get({
      security: [
        {
          scheme: "bearer",
          type: "http"
        }
      ],
      url: "/umbraco/uwis/api/v1/status",
      ...e
    });
  }
}
var P = Object.defineProperty, U = Object.getOwnPropertyDescriptor, g = (t) => {
  throw TypeError(t);
}, n = (t, e, a, o) => {
  for (var r = o > 1 ? void 0 : o ? U(e, a) : e, h = t.length - 1, d; h >= 0; h--)
    (d = t[h]) && (r = (o ? d(e, a, r) : d(r)) || r);
  return o && r && P(e, a, r), r;
}, G = (t, e, a) => e.has(t) || g("Cannot " + a), i = (t, e, a) => (G(t, e, "read from private field"), a ? a.call(t) : e.get(t)), p = (t, e, a) => e.has(t) ? g("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, a), l, _, v;
let u = class extends f {
  constructor() {
    super(...arguments), this._value = null, this._status = null, this._statusLoading = !1, p(this, l, async () => {
      var e;
      if (!((e = this.value) != null && e.WistiaAssetId)) {
        this._status = null;
        return;
      }
      this._statusLoading = !0;
      const { data: t } = await N.getStatus({ query: { assetId: this.value.WistiaAssetId } });
      this._statusLoading = !1, this._status = t, (this._status === s.UNKNOWN || this._status === s.PREPARING) && setTimeout(() => i(this, l).call(this), 5e3);
    }), p(this, _, (t) => {
      switch (t) {
        case s.PREPARING:
          return "warning";
        case s.ERRORED:
          return "danger";
        case s.READY:
          return "positive";
        default:
          return "default";
      }
    }), p(this, v, (t) => {
      switch (t) {
        case s.PREPARING:
          return "Preparing";
        case s.ERRORED:
          return "Errored";
        case s.READY:
          return "Ready";
        case s.NOT_FOUND:
          return "Not Found";
        default:
          return "Unknown";
      }
    });
  }
  set value(t) {
    this._value = t, i(this, l).call(this);
  }
  get value() {
    return this._value;
  }
  render() {
    return this.value ? c`<uui-ref-node name=${this.value.WistiaAssetId ?? "No asset id"} detail=${this.value.PlaybackUrl ?? "No playback URL"} readonly>
          <uui-icon slot="icon" name="icon-video"></uui-icon>
          ${this._statusLoading ? c`<uui-loader size="s" slot="tag"></uui-loader>` : c`<uui-tag size="s" slot="tag" color=${i(this, _).call(this, this._status)} @click=${i(this, l)}>${i(this, v).call(this, this._status)}</uui-tag>`}
        </uui-ref-node>` : c`<uui-tag look="placeholder">No video uploaded to Wistia</uui-tag>`;
  }
};
l = /* @__PURE__ */ new WeakMap();
_ = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
n([
  E()
], u.prototype, "value", 1);
n([
  m()
], u.prototype, "_value", 2);
n([
  m()
], u.prototype, "_status", 2);
n([
  m()
], u.prototype, "_statusLoading", 2);
u = n([
  R("uwis-property-editor-ui-wistia-sync")
], u);
const I = u;
export {
  u as UGumPropertyEditorUIWistiaSyncElement,
  I as default
};
//# sourceMappingURL=property-editor-ui-wistia-sync.element-DVj12vzL.js.map
