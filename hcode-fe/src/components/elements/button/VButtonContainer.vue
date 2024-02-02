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
         * Style justify-content 
         */
        justifyContent: {
            type: String,
            default: null
        },
    },
    computed: {
        styleComputed() {
            let style = `flex-direction: ${this.direction}; column-gap: ${this.gap}px; row-gap: ${this.gap}px;`;

            // Space between
            if (this.justifyContent) {
                style += `justify-content: ${this.justifyContent};`;
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