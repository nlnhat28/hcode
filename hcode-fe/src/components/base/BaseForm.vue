<script>
import enums from "@/enums/enums";
import loading from "@/mixins/loading-effect.js"
import { mapStores, mapState } from 'pinia';

export default {
    name: "BaseForm",
    mixins: [loading],
    props: {
        /**
         * Id của instance
         */
        instanceId: {
            type: [String, Number],
        },
        /**
         * (Props) Form mode {create | update}
         */
        formMode: {
            type: Number,
            default: enums.formMode.create,
            validator: (value) => {
                return [
                    enums.formMode.create,
                    enums.formMode.update,
                    enums.formMode.duplicate
                ].includes(
                    value
                );
            },
        },
    },
    data() {
        return {
            /**
             * Service
             */
            instanceService: null,
            /**
             * Mode of form
             */
            mode: this.formMode,
            /**
             * Đối tượng show trên form
             */
            instance: {},
            /**
             * Đối tượng gốc
             */
            originalInstance: {},
            /**
             * Flag check success response
             */
            isSuccessResponseFlag: true,
            /**
             * Message to show on dialog if invalid form
             */
            messageValidate: null,
            /**
             * Focused input
             */
            refFocus: null,
            /**
             * Focused error ref
             */
            refError: null,
            /**
             * Form item refs
             */
            refs: [],
            /**
             * Flag loading
             */
            isLoading: false,
            /**
             * Message hiện lên on toast
             */
            messageOnToast: null,
        };
    },
    async created() {
        this.$emitter.on("focusFormItem", (ref) => {
            this.focusErrorItem(ref);
        });
        this.$emitter.on("setMessageFormItem", (ref, errorMessage) => {
            this.setErrorMessage(ref, errorMessage);
        });

        await this.loadingEffect(async () => {
            await this.handleInstanceOnCreate();
        });
    },
    mounted() {
        // Gán các refs để validate
        this.refs = [];
    },
    unmounted() {
        this.$emitter.off("focusFormItem")
        this.$emitter.off("setMessageFormItem")
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
        /**
         * Reformat instance trước khi so sánh
         *
         * Author: nlnhat (02/07/2023)
         */
        reformatInstance() {
            return this.instance;
        },
        /**
         * Change title when change mode
         *
         * Author: nlnhat (02/07/2023)
         * @return {*} New title update or create
         */
        titleComputed() {
            switch (this.mode) {
                case this.$enums.formMode.create:
                    return this.$resources["vn"].createInstance;
                case this.$enums.formMode.update:
                    return this.$resources["vn"].updateInstance;
                case this.$enums.formMode.duplicate:
                    return this.$resources["vn"].duplicateInstance;
                default:
                    return this.$resources["vn"].createInstance;
            }
        },
    },
    methods: {
        /**
         * Handle instance on created()
         *
         * Author: nlnhat (05/07/2023)
         */
        async handleInstanceOnCreate() {
            switch (this.mode) {
                case this.$enums.formMode.create:
                    this.storeOriginalInstance();
                    break;
                case this.$enums.formMode.update:
                    await this.getInstance(this.instanceId)
                    break;
                case this.$enums.formMode.duplicate:
                    await this.getInstance(this.instanceId)
                    break;
                default:
                    break;
            }
        },
        /**
         * Lưu đối tượng gốc
         * 
         * Author: nlnhat (30/08/2023)
         */
        storeOriginalInstance() {
            this.originalInstance = this.$lodash.cloneDeep(this.instance);
        },
        /**
         * Lấy đối tượng theo id
         *
         * Author: nlnhat (02/07/2023)
         */
        async getInstance(id) {
            if (this.instanceService) {
                const response = await instanceService.get(id);
                if (response?.status == this.$enums.httpStatus.ok) {
                    this.instance = this.$lodash.cloneDeep(response.data.Data);
                    this.processResponseGetData();
                    this.storeOriginalInstance();
                }
            }
        },
        /**
         * Xử lý dữ liệu trả về
         */
        processResponseGetData() {

        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async createInstance() {
            try {
                const response = await instanceService.post(this.reformatInstance);
                if (response?.status == this.$enums.httpStatus.created) {
                    this.instance.InstanceId = response.data.Data;
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
            }
        },
        /**
         * Update instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async updateInstance() {
            try {
                const response = await instanceService.put(
                    this.instance.InstanceId,
                    this.reformatInstance
                );
                if (response?.status == this.$enums.httpStatus.ok) {
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
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
         * Save 
         *
         * Author: nlnhat (02/07/2023)
         */
        async onSave() {
            await this.loadingEffect(this.funcOnSave);
        },
        async funcOnSave() {
            switch (this.mode) {
                case this.$enums.formMode.create:
                    await this.createInstance();
                    break;
                case this.$enums.formMode.update:
                    await this.updateInstance();
                    break;
                case this.$enums.formMode.duplicate:
                    await this.createInstance();
                    break;
                default:
                    break;
            };
        },
        /**
         * Trước khi Validate
         */
        beforeValidate() {

        },
        /**
         * Xử lý thêm validate tại đây
         * 
         * Author: nlnhat1 (01/11/2023)
         */
        customValidate() {

        },
        /**
         * Check form is valid or invalid
         *
         * Author: nlnhat (02/07/2023)
         * @return {*} True if valid, false if invalid
         */
        async isValidForm() {
            try {
                await this.beforeValidate();
                this.messageValidate = null;
                this.refs.forEach((ref) => {
                    const message = ref.checkValidate();
                    if (message) {
                        this.messageValidate = message;
                        this.refError = ref;
                    }
                });
                this.customValidate();

                return this.messageValidate == null;
            } catch (error) {
                console.error(error);
                return false;
            }
        },
        /**
         * Khi click nút lưu hoặc gửi
         *
         * Author: nlnhat (02/07/2023)
         */
        async onClickSave() {
            try {
                if (await this.isValidForm()) {
                    this.isSuccessResponseFlag = false;
                    await this.onSave();
                    if (this.isSuccessResponseFlag == true) {
                        this.$emit("emitUpdateFocusedId", this.instance.InstanceId);
                        this.$emit("emitUpdateFocusedIds", [this.instance.InstanceId]);
                        this.$emit("emitReloadData");

                        if (this.messageOnToast) {
                            this.$ts.success(this.messageOnToast);
                        };

                        await this.afterSaveSuccess();

                        this.closeThis();
                    }
                } else {
                    this.$dl.error(this.messageValidate, this.focusRefError);
                    
                    this.afterSaveError();
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * On click button save and add (Cất và thêm)
         *
         * Author: nlnhat (02/07/2023)
         */
        async onClickSaveAdd() {
            try {
                if (await this.isValidForm()) {
                    this.isSuccessResponseFlag = false;
                    await this.onSave();
                    if (this.isSuccessResponseFlag == true) {
                        this.$emit("emitUpdateFocusedId", this.instance.InstanceId);
                        this.$emit("emitReloadData");
                        if (this.messageOnToast) {

                            this.$ts.success(this.messageOnToast);
                        };
                
                        await this.afterSaveSuccess();

                        this.resetThis();
                    }
                } else {
                    this.$dl.error(this.messageValidate, this.focusRefError);

                    await this.afterSaveError();
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Xử lý sau khi lưu thành công
         */
        afterSaveSuccess() {

        },
        /**
         * Xử lý sau khi lưu thất bại
         */
        afterSaveError() {

        },
        /**
         * Reset form 
         * 
         * Author: nlnhat (28/08/2023)
         */
        resetThis() {
            this.$emit("emitReRenderForm");
        },
        /**
         * Show confirm dialog
         *
         * Author: nlnhat (06/07/2023)
         * @param {string} message Content to show on dialog
         */
        showSaveConfirmDialog(message) {
            this.confirmDialog.content = message;
            this.confirmDialog.isShowed = true;
            this.focusOnButton();
        },
        /**
         * Handle when click save on confirm dialog
         *
         * Author: nlnhat (06/07/2023)
         */
        async onClickSaveConfirm() {
            this.confirmDialog.isShowed = false;
            await this.onClickSave();
        },
        /**
         * Handle click close this form
         *
         * Author: nlnhat (26/08/2023)
         */
        onClickCloseForm() {
            if (!this.sameObject(this.originalInstance, this.reformatInstance))
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
         * Handle shortcut keys on last button
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event on last button
         */
        handleShortKeyLastButton(event) {
            const code = this.$enums.keyCode;
            // Tab: Focus on first input
            if (event.keyCode == code.tab) {
                event.stopPropagation();
                event.preventDefault();
                this.focusFirstInput();
            }
        },
        /**
         * Focus on first input
         *
         * Author: nlnhat (25/07/2023)
         */
        focusFirstInput() {
            this.$refs[this.refFocus].focus();
        },
        /**
         * Focus on error item
         *
         * Author: nlnhat (04/08/2023)
         * @param {*} ref Ref name of error item
         */
        focusErrorItem(ref) {
            if (this.$refs[ref]) {
                this.$refs[ref].focus();
            }
        },
        /**
         * Focus on ref error
         *
         * Author: nlnhat (04/08/2023)
         */
        focusRefError() {
            if (this.refError) {
                this.refError.focus();
            }
        },
        /**
         * Set message for error item
         *
         * Author: nlnhat (04/08/2023)
         * @param {*} ref Ref name of error item
         * @param {*} errorMessage Error message
         */
        setErrorMessage(ref, errorMessage) {
            if (this.$refs[ref]) {
                this.$refs[ref].errorMessage = errorMessage;
            }
        },
        /**
         * On click help
         * 
         * Author: nlnhat (26/08/2023)
         */
        onClickHelp() {

        },
        /**
         * Ẩn form
         */
        closeThis() {

        },
    },
};
</script>
