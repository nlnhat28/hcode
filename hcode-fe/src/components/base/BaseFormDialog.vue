<script>
import enums from "@/enums/enums";
import { loadingEffect, handleResponse } from "@/mixins/mixins.js"
import { mapStores, mapState } from 'pinia';
import BaseForm from "./BaseForm.vue";
const formMode = enums.formMode;

export default {
    extends: BaseForm,
    name: "BaseForm",
    mixins: [loadingEffect, handleResponse],
    props: {
        /** Object từ cha truyền cho */
        object: {
            type: Object,
            default: null,
        },
        /** Dùng Object từ cha truyền cho */
        useObject: {
            type: Boolean,
            default: true,
        },
        /** Clone object trước khi gán cho instance */
        isCloneObject: {
            type: Boolean,
            default: true,
        }
    },
    data() {
        return {
            /** build buildDocumentTitle hay k */
            hasBuildDocumentTitle: false,
        };
    },
    async created() {
        await this.initOnCreated();
        await this.loadingEffect(async () => {
            await Promise.all([
                this.loadDataOnCreated(),
                this.customLoadDataOnCreated(),
            ]);
            await this.customInstanceOnCreated();
        });
        await this.afterLoadDataOnCreated();
    },
    unmounted() {
    },
    emits: [
        "emitReloadData",
        "emitReRenderForm",
        "emitUpdateFocusedId",
        "emitUpdateFocusedIds",
    ],
    watch: {
    },
    computed: {
    },
    methods: {
        /**
         * Khởi tạo dữ liệu data
         */
        async initOnCreated() {
        },
        /**
         * Load data
         * @virtual
         */
        async customLoadDataOnCreated() {
        },
        /**
         * Custom lại instance
         * @virtual
         */
        async afterLoadDataOnCreated() {
        },
        /**
         * Custom lại instance
         * @virtual
         */
        async customInstanceOnCreated() {
        },
        /**
         * Handle instance on created()
         *
         * Author: nlnhat (05/07/2023)
         */
        async loadDataOnCreated() {
            if (this.useObject) {
                this.instance = this.isCloneObject ? this.$cf.cloneDeep(this.object) : this.object;
                this.storeOriginalInstance();
            }
            else {
                switch (this.mode) {
                    case this.$enums.formMode.view:
                        await this.viewInstance(this.instanceId);
                        break;
                    case this.$enums.formMode.create:
                        this.initCreateInstance();
                        this.storeOriginalInstance();
                        break;
                    case this.$enums.formMode.update:
                        await this.getInstance(this.instanceId)
                        break;
                    case this.$enums.formMode.duplicate:
                        await this.getInstance(this.instanceId)
                        break;
                    default:
                        this.storeOriginalInstance();
                        break;
                }
            }
        },
        /**
         * Show dialog
         *
         * Author: nlnhat (04/07/2023)
         * @param {Object} config 
         */
        showDialog(config) {
        },
        /**
         * Close dialog
         *
         * Author: nlnhat (10/07/2023)
         */
        closeDialog() {
        },
        /**
         * Reset form 
         * 
         * Author: nlnhat (28/08/2023)
         */
        resetThis() {
            this.$emit("reset");
        },
        /**
         * Handle click close this form
         *
         * Author: nlnhat (26/08/2023)
         */
        onClickCloseForm() {
            if (!this.sameObject(this.originalInstance, this.reformatInstance()))
                this.showSaveConfirmDialog(this.$resources["vn"].saveChangeConfirm);
            else
                this.closeThis();
        },
        /**
         * Handle shortcut keys
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;

            // Ctrl + S || Ctrl + F8: Cất
            if (
                ((event.ctrlKey && event.keyCode == code.s) ||
                    (event.ctrlKey && event.keyCode == code.f8)) &&
                !event.shiftKey
            ) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickSave();
            }
            // Ctrl + Shift + N: Cất và thêm
            else if (event.ctrlKey && event.shiftKey && event.keyCode == code.s) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickSaveAdd();
            }
            // Esc: Đóng form
            else if (event.keyCode == code.esc) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickCloseForm();
            }
            // F1: Help
            else if (event.keyCode == code.f1) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickHelp();
            }
        },
        /**
         * Ẩn form
         */
        closeThis() {
        },
    },
};
</script>
