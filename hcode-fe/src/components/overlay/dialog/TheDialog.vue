<template>
    <v-dialog
        class="v-dialog"
        v-model:visible="dialog.visible"
        modal
        :header="dialog.header"
        :draggable="false"
    >
        <template #closeIcon>
            <v-icon icon="far fa-circle-xmark" />
        </template>
        <div class="v-dialog__content">
            <v-icon
                class="v-dialog__icon"
                :icon="dialog.icon.icon"
                :color="dialog.icon.color"
                :size="36"
            />
            <div class="v-dialog__message">
                {{ dialog.message }}
            </div>
        </div>
        <template #footer>
            <v-button-container flexEnd>
                <v-button
                    v-for="(button, index) in dialog.buttons"
                    :key="index"
                    :label=button.label
                    :icon=button.icon
                    :severity="button.severity"
                    :text="button.text ?? false"
                    :outlined="button.outlined ?? false"
                    :autofocus="button.autofocus || index == dialog.buttons.length - 1"
                    :loading="button.loading"
                    :disabled="disabledAllButton"
                    size="small"
                    @click="onClickButton(button)"
                />
            </v-button-container>
        </template>
    </v-dialog>
</template>

<script>
export default {
    name: "TheDialog",
    data() {
        return {
            /**
             * Object dialog
             */
            dialog: {
                visible: false,
                header: null,
                icon: null,
                message: null,
                buttons: [],
                callback: null,
            },
            /**
             * Disable các button khi đang loading
             */
            disabledAllButton: false,
        }
    },
    computed: {

    },
    created() {
        this.$emitter.on("dialogError", (message, callback, header) => {
            this.assignDialog(message, callback, header, null, this.$enums.dialogType.error)
        });
        this.$emitter.on("dialogInfo", (message, header) => {
            this.assignDialog(message, callback, header, null, this.$enums.dialogType.info)
        });
        this.$emitter.on("dialogWarn", (message, buttons, header) => {
            this.assignDialog(message, null, header, buttons, this.$enums.dialogType.warn)
        });
        this.$emitter.on("dialogConfirm", (message, buttons, header) => {
            this.assignDialog(message, null, header, buttons, this.$enums.dialogType.confirm)
        });
    },
    unmounted() {
        this.$emitter.off("dialogError");
        this.$emitter.off("dialogWarn");
        this.$emitter.off("dialogInfo");
        this.$emitter.off("dialogConfirm");
    },
    methods: {
        /**
         * Gán thông tin dialog
         * @param {*} header Tiêu để
         * @param {*} message Nội dung
         * @param {*} dialogType 
         */
        assignDialog(message, callback, header, buttons, dialogType) {
            switch (dialogType) {
                // Nếu dialog thông báo lỗi
                case this.$enums.dialogType.error:
                    this.dialog.header = header ?? this.$t("com.error");
                    this.dialog.icon = {
                        icon: "fa fa-circle-exclamation",
                        color: "error"
                    };
                    this.dialog.buttons = [
                        {
                            label: this.$t("com.gotIt"),
                            icon: "fa fa-check",
                            click: callback,
                        }
                    ]
                    break;
                // Nếu dialog thông báo
                case this.$enums.dialogType.info:
                    this.dialog.header = header ?? this.$t("com.info");
                    this.dialog.icon = {
                        icon: "fa fa-circle-exclamation",
                        color: "info"
                    };
                    this.dialog.buttons = [
                        {
                            label: this.$t("com.gotIt"),
                            icon: "fa fa-check",
                            click: callback,
                        }
                    ]
                    break;
                // Nếu dialog cảnh báo
                case this.$enums.dialogType.warn:
                    this.dialog.header = header ?? this.$t("com.warn");
                    this.dialog.icon = {
                        icon: "fa fa-triangle-exclamation",
                        color: "warn"
                    };
                    this.dialog.buttons = buttons ? buttons : [
                        {
                            label: this.$t("com.gotIt"),
                            icon: "fa fa-check",
                            click: callback,
                        }
                    ]
                    break;
                // Nếu dialog xác nhận
                case this.$enums.dialogType.confirm:
                    this.dialog.header = header ?? this.$t("com.confirm");
                    this.dialog.icon = {
                        icon: "fa fa-circle-question",
                        color: "info"
                    };
                    this.dialog.buttons = buttons ? buttons : [
                        {
                            label: this.$t("com.ok"),
                            icon: "fa fa-check",
                            click: callback,
                        }
                    ]
                    break;
                default:
                    break;
            };
            this.dialog.message = message;
            this.dialog.visible = true;
        },
        /**
         * Xử lý click button
         * @param {*} button 
         */
        async onClickButton(button) {
            if (button.click) {
                try {
                    this.disabledAllButton = true;
                    button.loading = true;
                    await button.click();
                }
                finally {
                    button.loading = false;
                    this.disabledAllButton = false;
                }
            }
            this.dialog.visible = false;
        },
    }
};
</script>
<style>
@import "./dialog.css";
</style>