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
import VButton from "@/components/elements/button/VButton.vue";
import VButtonContainer from "@/components/elements/button/VButtonContainer.vue";
import VCodeEditor from "@/components/elements/code-editor/VCodeEditor.vue";
import VCombobox from "@/components/elements/combobox/VCombobox.vue";
import VDialog from "@/components/elements/dialog/VDialog.vue";
import VEditor from "@/components/elements/editor/VEditor.vue";
import VFilter from "@/components/elements/filter/VFilter.vue";
import VFormBody from "@/components/elements/form/VFormBody.vue";
import VFormColumn from "@/components/elements/form/VFormColumn.vue";
import VFormGroup from "@/components/elements/form/VFormGroup.vue";
import VFormItem from "@/components/elements/form/VFormItem.vue";
import VFormRow from "@/components/elements/form/VFormRow.vue";
import VIcon from "@/components/elements/icon/VIcon.vue";
import VInputText from "@/components/elements/input/VInputText.vue";
import VInputGroup from "@/components/elements/input/VInputGroup.vue";
import VLogoHCode from "@/components/elements/logo/VLogoHcode.vue";
import VMaskDialog from "@/components/elements/mask/VMaskDialog.vue";
import VMaskLoading from "@/components/elements/mask/VMaskLoading.vue";
import VNoContent from "@/components/elements/mask/VNoContent.vue";
import VNav from "@/components/elements/nav/VNav.vue";
import VNavItem from "@/components/elements/nav/VNavItem.vue";
import VPassword from "@/components/elements/input/VPassword.vue";
import VLoader from "@/components/elements/loader/VLoader.vue";
import VLoaderLock from "@/components/elements/loader/VLoaderLock.vue";
import VPagination from "@/components/elements/pagination/VPagination.vue";
import VPageNumber from "@/components/elements/pagination/VPageNumber.vue";
import VSearchBox from "@/components/elements/search/VSearchBox.vue";
import VSearchResult from "@/components/elements/search/VSearchResult.vue";
import VSkeleton from "@/components/elements/skeleton/VSkeleton.vue";
import VSpinner from "@/components/elements/loader/VSpinner.vue";
// import VSplitter from "@/components/elements/splitter/VSplitter.vue";
// import VSplitterPanel from "@/components/elements/splitter/VSplitterPanel.vue";
import VSplitter from "primevue/splitter";
import VSplitterPanel from "primevue/splitterpanel";
import VTooltip from "@/components/elements/tooltip/VTooltip.vue";
import VRating from "@/components/elements/rating/VRating.vue";
import VReaction from "@/components/elements/reaction/VReaction.vue";
import VTabView from "primevue/tabview";
import VTabPanel from "primevue/tabpanel";
import VTable from "@/components/elements/table/VTable.vue";
import VTag from "@/components/elements/tag/VTag.vue";
import VTh from "@/components/elements/table/VTh.vue";
import VTd from "@/components/elements/table/VTd.vue";
import VTr from "@/components/elements/table/VTr.vue";
/**
 * Others
 */
import CountDown from "keep-countdown";
// import 'keep-countdown/index.css'

app.component("v-button", VButton);
app.component("v-button-container", VButtonContainer);
app.component("v-code-editor", VCodeEditor);
app.component("v-combobox", VCombobox);
app.component("v-countdown-simple", CountDown);
app.component("v-dialog", Dialog);
app.component("v-editor", VEditor);
app.component("v-form-body", VFormBody);
app.component("v-form-column", VFormColumn);
app.component("v-form-group", VFormGroup);
app.component("v-form-item", VFormItem);
app.component("v-form-row", VFormRow);
app.component("v-filter", VFilter);
app.component("v-icon", VIcon);
app.component("v-input-group", VInputGroup);
app.component("v-input-text", VInputText);
app.component("v-logo-hcode", VLogoHCode);
app.component("v-mask-loading", VMaskLoading);
app.component("v-mask-dialog", VMaskDialog);
app.component("v-no-content", VNoContent);
app.component("v-nav", VNav);
app.component("v-nav-item", VNavItem);
app.component("v-password", VPassword);
app.component("v-loader", VLoader);
app.component("v-loader-lock", VLoaderLock);
app.component("v-pagination", VPagination);
app.component("v-page-number", VPageNumber);
app.component("v-spinner", VSpinner);
app.component("v-splitter", VSplitter);
app.component("v-splitter-panel", VSplitterPanel);
app.component("v-search-box", VSearchBox);
app.component("v-search-result", VSearchResult);
app.component("v-skeleton", VSkeleton);
app.component("v-rating", VRating);
app.component("v-reaction", VReaction);
app.component("v-tab-view", VTabView);
app.component("v-tab-panel", VTabPanel);
app.component("v-tag", VTag);
app.component("v-table", VTable);
app.component("v-th", VTh);
app.component("v-td", VTd);
app.component("v-tr", VTr);
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
import clickOutside from "@/directives/click-outside.js";
import contextmenuOutside from "@/directives/contextmenu-outside.js";

app.directive("tooltip", tooltip);
app.directive("click-outside", clickOutside);
app.directive("contextmenu-outside", contextmenuOutside);

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
