/**
 * Import
 */
import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import "@/css/main.css";
import "@/helper/prototype.js";
/**
 * Create
 */
const pinia = createPinia();
const app = createApp(App);

/**
 * Components
 */
/**
 * PrimeVue
 */
import PrimeVue from "primevue/config";
import "primevue/resources/themes/lara-light-indigo/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";
import Button from "primevue/button";
import Dialog from "primevue/dialog";
/**
 * Custom
 */
import VButtonContainer from "@/components/elements/button/VButtonContainer.vue";
import VIcon from "@/components/elements/icon/VIcon.vue";
import VInputText from "@/components/elements/input/VInputText.vue";
import VInputGroup from "@/components/elements/input/VInputGroup.vue";
import VLogoHCode from "@/components/elements/logo/VLogoHcode.vue";
import VNav from "@/components/elements/nav/VNav.vue";
import VNavItem from "@/components/elements/nav/VNavItem.vue";
import VPassword from "@/components/elements/input/VPassword.vue";
import VSpinner from "@/components/elements/spinner/VSpinner.vue";
import VTooltip from "@/components/elements/tooltip/VTooltip.vue";
/**
 * Simple code editor
 */
import CodeEditor from "simple-code-editor";
import "simple-code-editor/themes/themes.css";

app.component("v-button", Button);
app.component("v-btn-container", VButtonContainer);
app.component("v-code-editor", CodeEditor);
app.component("v-dialog", Dialog);
app.component("v-icon", VIcon);
app.component("v-input-group", VInputGroup);
app.component("v-input-text", VInputText);
app.component("v-logo-hcode", VLogoHCode);
app.component("v-nav", VNav);
app.component("v-nav-item", VNavItem);
app.component("v-password", VPassword);
app.component("v-spinner", VSpinner);
app.component("v-tooltip", VTooltip);

/**
 * Global
 */
import path from "@/router/path.js";
import emitter from "tiny-emitter/instance";
import commonFuction from "@/helper/common-function.js";
import convert from "@/helper/convert.js";
import format from "@/helper/format.js";
import reformat from "@/helper/reformat.js";
import enums from "@/enums/enums.js";
import toast from "@/components/overlay/toast/toast.js";
import dialog from "@/components/overlay/dialog/dialog.js";
import validate from "@/helper/validate.js";

app.config.globalProperties.$emitter = emitter;
app.config.globalProperties.$enums = enums;
app.config.globalProperties.$path = path;
app.config.globalProperties.$cf = commonFuction;
app.config.globalProperties.$cv = convert;
app.config.globalProperties.$fm = format;
app.config.globalProperties.$rfm = reformat;
app.config.globalProperties.$ts = toast;
app.config.globalProperties.$dl = dialog;
app.config.globalProperties.$vld = validate;
/**
 * Directives
 */
import tooltip from "@/directives/tooltip.js";

app.directive("tooltip", tooltip);

/**
 * Plugin
 */
import router from "@/router/router.js";
import i18n from "@/i18n/i18n.js";
import ToastService from "primevue/toastservice";

app.use(PrimeVue, { ripple: true });
app.use(ToastService);
app.use(pinia);
app.use(router);
app.use(i18n);

/**
 * Mount
 */
app.mount("#app");

export default app;
