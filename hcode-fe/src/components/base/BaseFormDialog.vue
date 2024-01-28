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
            /** Show dialog */
            isVisible: false,
        };
    },
    emits: [
        "close",
        "reset",
        "reload",
    ],
    watch: {
    },
    computed: {
    },
    methods: {

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
         * Xử lý dữ liệu trả về
         */
        processResponseLoad(response) {
            if (this.$res.isSuccess(response)) {
                this.show();
            }
            else {
                this.close();
            }
        },
        /**
         * Show dialog
         *
         * Author: nlnhat (04/07/2023)
         * @param {Object} config 
         */
        show(options) {
            this.isVisible = true;
        },
        /**
         * Close dialog
         *
         * Author: nlnhat (10/07/2023)
         */
        close() {
            this.isVisible = false;
            this.$emit("close");
        },
        /**
         * Reload list 
         * 
         * Author: nlnhat (28/08/2023)
         */
        reload() {
            this.$emit("reload");
        },
        /**
         * Reset form 
         * 
         * Author: nlnhat (28/08/2023)
         */
        reset() {
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
    },
};
</script>
