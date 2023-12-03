<template>
    <div
        :class="[
            'icon-container',
            { 'cursor-pointer': applyPointer },
            color
        ]"
        :style="`width: ${size}px; height: ${size}px;`"
    >
        <i :class="[
            icon,
            'icon',
            { 'cursor-pointer': applyPointer },
        ]"></i>
    </div>
</template>
<script>
export default {
    name: 'VIcon',
    props: {
        /**
         * Icon class name: ex: fa fa-user
         */
        icon: {
            type: String
        },
        /**
         * Pointer khi hover
         */
        applyPointer: {
            type: Boolean,
            default: false
        },
        /**
         * Size of container
         */
        size: {
            type: Number,
            default: 24,
        },
        /**
         * 
         */
        color: {
            type: String,
            default: 'dark',
            validator: (value) => {
                return [
                    'dark',
                    'light',
                    'success',
                    'error',
                    'danger',
                    'primary',
                    'warn',
                    'info'
                ].includes(value)
            }
        }
    },
}
</script>
<style scoped>
@import './icon.css';
</style>