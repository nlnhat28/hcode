<script>
import enums from "@/enums/enums";
import { loadingEffect, handleResponse } from "@/mixins/mixins.js"
import { mapStores, mapState } from 'pinia';
const formMode = enums.formMode;

export default {
    name: "BaseForm",
    mixins: [loadingEffect, handleResponse],
    props: {
        /**
         * Id của instance
         */
        id: {
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
            cfg: {
                /** Path */
                formPath: '',
                /** Path gọi đến form này. Có gì còn callback */
                callbackPath: '',
                /** tên entity */
                entity: '',
                /** tên phân hệ */
                subSysName: 'Form',
            },
            /** Service */
            instanceService: null,
            /** Mode of form */
            mode: this.formMode,
            /*** Đối tượng show trên form */
            instance: {},
            /** id của instance */
            instanceId: this.id,
            /** Đối tượng gốc */
            originalInstance: {},
            /** Flag check success response */
            isSuccessResponseFlag: true,
            /** Message to show on dialog if invalid form */
            messageValidate: null,
            /** Focused input */
            refFocus: null,
            /** Focused error ref */
            refError: null,
            /** Form item refs */
            refs: [],
            /** Flag loading */
            isLoading: false,
            /** Message hiện lên on toast */
            messageOnToast: null,
            /** Title */
            documentTitle: null,
            /** build buildDocumentTitle hay k */
            hasBuildDocumentTitle: true,
        };
    },
    async created() {
        if (this.hasBuildDocumentTitle) {
            document.title = this.$cf.documentTitle(this.documentTitle);
        }
        
        let id = this.$route.params.id;

        if (id == null) {
            this.mode = formMode.create;
            this.instance = {};
        }
        else {
            this.mode = formMode.update;
            this.instanceId = id;
        }

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
    mounted() {
        // Gán các refs để validate
        this.refs = [];
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
        /**
         * Reformat instance trước khi lưu
         *
         * Author: nlnhat (02/07/2023)
         */
        reformatInstance() {
            return this.instance;
        },
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
            switch (this.mode) {
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
        },
        /**
         * Khởi tạo instance nếu là form tạo mới
         * @virtual
         */
        initCreateInstance() {
        },
        /**
         * Về màn callback path
         */
        goCallbackPath() {
            if (this.cfg.callbackPath) {
                this.$router.push(this.cfg.callbackPath);
            }
            else {
                console.warning("DEV chưa cấu hình cfg.callbackPath")
            }
        },
        /**
         * Về màn form path
         */
        goFormPath() {
            if (this.cfg.formPath) {
                this.$router.push(this.$cf.combineRoute(this.cfg.formPath));
            }
            else if (this.cfg.callbackPath) {
                this.$router.push(this.cfg.callbackPath);
            }
            else {
                console.warning("DEV chưa cấu hình cfg.callbackPath")
            }
        },
        /**
         * Lưu đối tượng gốc
         * 
         * Author: nlnhat (30/08/2023)
         */
        storeOriginalInstance() {
            this.originalInstance = this.$cf.cloneDeep(this.instance);
        },
        /**
         * Lấy đối tượng theo id
         *
         * Author: nlnhat (02/07/2023)
         */
        async getInstance(id) {
            if (this.instanceService) {
                const response = await this.instanceService.get(id);
                if (this.$res.isSuccess(response)) {
                    if (!this.$cf.isEmptyObject(response.Data)) {
                        this.instance = this.$cf.cloneDeep(response.Data);
                        this.storeOriginalInstance();
                        this.documentTitle = this.instance[`${this.cfg.entity}Name`] + " - " + this.cfg.subSysName;
                        document.title = this.$cf.documentTitle(this.documentTitle);
                    }
                    else {
                        this.$dl.error(this.$t('msg.cannotFindRecord'), this.goFormPath)
                    }
                }
                else {
                    this.handleError(response, this.goCallbackPath)
                }
                this.processResponseGet(response);
            }
        },
        /**
         * Xử lý dữ liệu trả về
         */
        processResponseGet(response) {
        },
        /**
         * Create new instance
         *
         * Author: nlnhat (02/07/2023)
         */
        async createInstance() {
            try {
                const response = await this.instanceService.post(this.reformatInstance);
                if (this.$res.isSuccess(response)) {
                    this.instanceId = response.Data;
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                    this.handleError(response);
                }
                this.processResponseCreate(response);

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
                const response = await this.instanceService.put(
                    this.instanceId,
                    this.reformatInstance
                );
                if (this.$res.isSuccess(response)) {
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                    this.handleError(response);
                }
                this.processResponseUpdate(response);
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
                    if (ref) {
                        const message = ref.checkValidate();
                        if (message) {
                            this.messageValidate = message;
                            this.refError = ref;
                        }
                    }
                });
                this.customValidate();

                return this.messageValidate == null;
            } catch (error) {
                console.error(error);
                this.messageValidate == null
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
                await this.beforeDoSave();
                await this.doSave();
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Hàm xử lý save
         */
        async doSave() {
            if (await this.isValidForm()) {
                await this.customBeforeSave();
                this.isSuccessResponseFlag = false;
                await this.onSave();
                if (this.isSuccessResponseFlag == true) {

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
        showToastSaveSuccess() {
            let message = '';
            switch (this.mode) {
                case this.$enums.formMode.create:
                    message = this.$t('msg.createSuccess');
                    break;
                case this.$enums.formMode.update:
                    message = this.$t('msg.updateSuccess');
                    break;
                case this.$enums.formMode.duplicate:
                    message = this.$t('msg.duplicateSuccess');
                    break;
                default:
                    message = this.$t('msg.saveSuccess');
                    break;
            };

            if (!this.$cf.isEmptyString(message)) {
                this.$ts.success(message);
            }
        },
        /**
         * Trước khi doSave()
         * @virtual
         */
        beforeDoSave() {

        },
        /**
         * Response sau khi call api createInstance
         * @virtual
         */
        processResponseCreate(response) {
        },
        /**
         * Response sau khi call api updateInstance
         * @virtual
         */
        processResponseUpdate(response) {
        },
        async customBeforeSave() {
        },
        /**
         * Xử lý sau khi lưu thành công
         */
        async afterSaveSuccess() {
        },
        /**
         * Xử lý sau khi lưu thất bại
         */
        async afterSaveError() {
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
