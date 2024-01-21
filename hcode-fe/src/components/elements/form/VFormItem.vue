<template>
    <div :class="['form__item', { 'disabled': isDisabled }, { 'readonly': isReadOnly }]">
        <div
            class="form__label"
            v-tooltip:right="title"
        >
            <label class="label"> {{ label }} </label>
            <div
                class="required"
                v-if="isRequired"
            >*</div>
        </div>
        <slot ref="slot"></slot>
    </div>
</template>
<script>
export default {
    name: 'VFormItem',
    props: {
        /**
         * Label of form item
         */
        label: {
            type: String,
        },
        /**
         * Description about item
         */
        title: {
            type: String,
        },
        /**
         * Required of not
         */
        isRequired: {
            type: Boolean,
            default: false,
        },
        /**
         * Disabled
         */
        isDisabled: {
            type: Boolean,
            default: false,
        },
        /**
         * Readonly
         */
        isReadOnly: {
            type: Boolean,
            default: false,
        }
    },
}
</script>
<style scoped>
@import './form.css';
</style>