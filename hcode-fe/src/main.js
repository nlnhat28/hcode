/**
 * Import
 */
import { createApp } from "vue";
import App from "./App.vue";
import "@/css/main.css";
import "@/helper/prototype.js";
/**
 * Create
 */
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
import VMaskLoading from "@/components/elements/mask/VMaskLoading.vue";
import VNav from "@/components/elements/nav/VNav.vue";
import VNavItem from "@/components/elements/nav/VNavItem.vue";
import VPassword from "@/components/elements/input/VPassword.vue";
import VSpinner from "@/components/elements/loader/VSpinner.vue";
import VLoader from "@/components/elements/loader/VLoader.vue";
import VTooltip from "@/components/elements/tooltip/VTooltip.vue";
/**
 * Others
 */
import CodeEditor from "simple-code-editor";
import "simple-code-editor/themes/themes.css";
import CountDown from 'keep-countdown'
// import 'keep-countdown/index.css'

app.component("v-button", Button);
app.component("v-btn-container", VButtonContainer);
app.component("v-code-editor", CodeEditor);
app.component("v-countdown-simple", CountDown);
app.component("v-dialog", Dialog);
app.component("v-icon", VIcon);
app.component("v-input-group", VInputGroup);
app.component("v-input-text", VInputText);
app.component("v-logo-hcode", VLogoHCode);
app.component("v-mask-loading", VMaskLoading);
app.component("v-nav", VNav);
app.component("v-nav-item", VNavItem);
app.component("v-password", VPassword);
app.component("v-spinner", VSpinner);
app.component("v-loader", VLoader);
app.component("v-tooltip", VTooltip);

/**
 * Global
 */
import path from "@/router/path.js";
import emitter from "tiny-emitter/instance";
import commonFunction from "@/helper/common-function.js";
import convert from "@/helper/convert.js";
import format from "@/helper/format.js";
import reformat from "@/helper/reformat.js";
import enums from "@/enums/enums.js";
import toast from "@/components/overlay/toast/toast.js";
import dialog from "@/components/overlay/dialog/dialog.js";
import validate from "@/helper/validate.js";
import regex from "@/helper/regex.js";
import lodash from "lodash";

app.config.globalProperties.$emitter = emitter;
app.config.globalProperties.$enums = enums;
app.config.globalProperties.$path = path;
app.config.globalProperties.$cf = commonFunction;
app.config.globalProperties.$cv = convert;
app.config.globalProperties.$fm = format;
app.config.globalProperties.$rfm = reformat;
app.config.globalProperties.$ts = toast;
app.config.globalProperties.$dl = dialog;
app.config.globalProperties.$vld = validate;
app.config.globalProperties.$lodash = lodash;
app.config.globalProperties.$reg = regex;
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
import { createPinia } from "pinia";
import { createPersistedState } from "pinia-plugin-persistedstate";

const pinia = createPinia();
pinia.use(
    createPersistedState({
        key: (id) => `__persisted__${id}`,
    })
);
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
