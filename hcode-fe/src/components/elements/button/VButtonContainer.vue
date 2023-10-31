<template>
    <div
        class="v-button-container"
        :style="styleComputed"
    >
        <slot></slot>
    </div>
</template>
<script>
export default {
    name: "VButtonContainer",
    props: {
        /**
         * Khoảng cách
         */
        gap: {
            type: Number,
            default: 12,
        },
        /**
         * Hướng column?
         */
        column: {
            type: Boolean,
            default: false
        }
    },
    computed: {
        styleComputed() {
            if (!this.column) {
                return `column-gap: ${this.gap}px`;
            }
            return `flex-direction: column; row-gap: ${this.gap}px`;
        }
    }
}
</script>
<style scoped>
.v-button-container {
    display: flex;
}
</style>