<template>
    <v-icon
        :class="{
            'v-spinner': true,
        }"
        :color=color
        icon="fa-duotone fa-spinner fa-spin-pulse"
    ></v-icon>
</template>
<script>
export default {
    name: 'VSpinner',
    props: {
        /**
         * Color
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
                    'primary'
                ].includes(value)
            }
        }
    }
}
</script>
<style scoped>
/* @import './spinner.css'; */
</style>