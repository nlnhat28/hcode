<template>
    <div class="v-input-text">
        <span
            :class="[
                'input-wrapper',
                { 'p-input-icon-left': icon != null },
                { 'p-input-icon-right': innerAction != null },
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
                :class="[
                    'p-inputtext p-component',
                    { 'p-invalid': errorMessage != null },
                    { 'p-warn': warnMessage != null && errorMessage == null },
                    { 'transparent': isTransparent }
                ]"
                :placeholder="placeholderComputed"
                :disabled="isDisabled"
                :readonly="isReadOnly"
                :title="tooltip || innerValue"
                :style="`text-align: ${textAlign};`"
            />
            <div
                v-if="innerAction"
                :class="[
                    'input-icon-right',
                    { '.always-show': isShowActionIcon }
                ]"
            >
                <v-icon
                    v-if="!isLoading && !isSuccess & !isError"
                    v-tooltip="innerAction?.tooltip"
                    applyPointer
                    :icon="innerAction?.icon"
                    @click="onAction"
                />
                <v-spinner
                    v-if="isLoading && !isSuccess && !isError"
                    color="primary"
                />
                <v-icon
                    v-if="isSuccess"
                    icon="fa fa-check"
                    color="primary"
                ></v-icon>
                <v-icon
                    v-if="isError"
                    icon="fa fa-xmark"
                    color="error"
                ></v-icon>
            </div>
        </span>
    </div>
</template>

<script>
import InputText from "primevue/inputtext";
import BaseInput from "./BaseInput.vue";

export default {
    name: "VInputText",
    extends: BaseInput,
    components: {
        InputText,
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
    props: {
        /**
         * Cho phép copy
         */
        hasCopy: {
            type: Boolean,
            default: false
        },
        /**
         * Cho phép clear
         */
        hasClear: {
            type: Boolean,
            default: false
        }
    },
    data() {

    },
    created() {
        this.customOnChangeValue();
    },
    methods: {
        /**
         * Custom khi dữ liệu thay đổi
         */
        customOnChangeValue() {
            // Config nếu có copy
            if (this.hasCopy) {
                if (this.$cf.isNullString(this.innerValue)) {
                    this.innerAction = null;
                }
                else {
                    this.innerAction = {
                        icon: 'far fa-copy',
                        tooltip: this.$t('com.copy'),
                        method: () => {
                            return this.$cf.copyToClipboard(this.innerValue)
                        },
                        hasLoading: true,
                    }
                }
            }
            // Config nếu có clear
            else if (this.hasClear) {
                if (this.$cf.isNullString(this.innerValue)) {
                    this.innerAction = null;
                }
                else {
                    this.innerAction = {
                        icon: 'fa fa-xmark',
                        method: () => {
                            this.innerValue = null;
                        },
                        hasLoading: false,
                    }
                }
            }
        }
    }
}
</script>
<style scoped>
@import "./input.css";
</style>