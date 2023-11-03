<template>
    <div class="v-input-text">
        <span
            :class="[
                'input-wrapper',
                'p-input-icon-right',
                { 'p-input-icon-left': icon != null },
                { 'tooltip--error': errorMessage != null },
                { 'tooltip--warn': warnMessage != null && errorMessage == null }
            ]"
            v-tooltip:top="tooltipComputed"
        >
            <div class="input-icon-left">
                <v-icon
                    v-if="icon"
                    :icon="icon"
                />
            </div>
            <input
                ref="refEl"
                v-model="innerValue"
                :autofocus="isFocused"
                :class="[
                    'p-inputtext p-component',
                    { 'p-invalid': errorMessage != null },
                    { 'p-warn': warnMessage != null && errorMessage == null },
                ]"
                :placeholder="placeholderComputed"
                :type="showPassword ? 'text' : 'password'"
                :title="titleComputed"
                :disabled="isDisabled"
                :readonly="isReadOnly"
                :style="`text-align: ${textAlign};`"
            />
            <div class="input-icon-right always-show">
                <v-icon
                    v-if="isConfirm && correctComputed"
                    icon="fa fa-check"
                    color="primary"
                ></v-icon>
                <v-icon
                    v-if="!isConfirm || (isConfirm && !correctComputed)"
                    v-tooltip="showPassword ? $t('com.hidePassword') : $t('com.showPassword')"
                    applyPointer
                    :icon="showPassword ? 'far fa-eye' : 'far fa-eye-slash'"
                    @click="onAction"
                />
            </div>
        </span>
    </div>
</template>

<script>
import InputText from "primevue/inputtext";
import BaseInput from "./BaseInput.vue";

export default {
    name: "VPassword",
    extends: BaseInput,
    components: {
        InputText,
    },
    props: {
        /**
         * Là component confirm
         */
        isConfirm: {
            type: Boolean,
            default: false
        },
        /**
         * Đã nhập lại đúng mật khẩu
         */
        password: {
            type: [String, Number],
        },
    },
    expose: [
        'checkValidate',
        'focus',
        'select',
        'errorMessage',
        'clearErrorMessage',
        'setErrorMessage',
        'onAction'
    ],
    data() {
        return {
            /**
             * Hiện text
             */
            showPassword: false,
        }
    },
    computed: {
        /**
         * Title cho password
         */
        titleComputed() {
            return this.isConfirm ? null : this.$t("com.passwordTooltip");
        },
        /**
         * Đã nhập đúng mật khẩu
         */
        correctComputed() {
            return !this.$cf.isNullString(this.innerValue) && this.innerValue == this.password;
        },
    },
    methods: {
        /**
         * Show password
         */
        onAction() {
            this.showPassword = !this.showPassword;
        }
    }
}
</script>
<style scoped>
@import "./input.css";
</style>