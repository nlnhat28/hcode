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
         * Hướng
         */
        direction: {
            type: String,
            default: 'row'
        },
        /**
         * Flex-end 
         */
        flexEnd: {
            type: Boolean,
            default: false
        },
    },
    computed: {
        styleComputed() {
            let style = `flex-direction: ${this.direction}; column-gap: ${this.gap}px; row-gap: ${this.gap}px;`;

            // Flex end
            if (this.flexEnd) {
                style += 'justify-content: flex-end;'
            };

            return style;
        }
    }
}
</script>
<style scoped>
.v-button-container {
    display: flex;
    align-items: center;
}
</style>