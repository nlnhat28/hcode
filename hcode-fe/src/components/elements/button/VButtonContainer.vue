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
        isColumn: {
            type: Boolean,
            default: false
        },
        /**
         * Flex-end 
         */
        flexEnd: {
            type: Boolean,
            default: false  
        }
    },
    computed: {
        styleComputed() {
            let style = ''
            if (!this.isColumn) {
                style = `column-gap: ${this.gap}px; `;
            }
            else {
                style = `flex-direction: column; row-gap: ${this.gap}px; `;
            };

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